using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Errors;
using Capychef.Food.Domain.Interfaces;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;
using Capychef.Storage.Domain.Entities;
using Capychef.Storage.Domain.Errors;
using Capychef.Storage.Domain.Interfaces;

namespace Capychef.Storage.Services;

public class BatchService(
    CapychefDbContext dbContext,
    IBatchRepository batchRepository,
    IBatchModificationHistoryRepository batchModificationHistoryRepository,
    IFoodRepository foodRepository,
    IStorageSpaceRepository storageSpaceRepository) : IBatchService
{
    public async Task<Result<BatchDto>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<BatchDto>(validationError);

        var food = await foodRepository.GetTrackedFoodById(cmd.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchDto>(new NotFoundError(EntityType.Food, cmd.FoodId));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.GetHouseholdId()))
            return new Result<BatchDto>(new NotFoundError(EntityType.StorageSpace, cmd.FoodId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<BatchDto>(new NotFoundError(EntityType.FoodUoM,
                $"{cmd.FoodUoMId} for food {cmd.FoodId}"));

        DateTime? bestBeforeDate = null;

        if (cmd.BestBeforeDate.HasValue)
            bestBeforeDate = cmd.BestBeforeDate.Value;
        else if (food.HouseholdFoodDetails is { DaysUntilBestBefore: not null })
            bestBeforeDate = DateTime.UtcNow.AddDays(food.HouseholdFoodDetails.DaysUntilBestBefore.Value);

        DateTime? expirationDate = null;

        if (cmd.ExpirationDate.HasValue)
            expirationDate = cmd.ExpirationDate.Value;
        else if (food.HouseholdFoodDetails is { DaysUntilExpiration: not null })
            expirationDate = DateTime.UtcNow.AddDays(food.HouseholdFoodDetails.DaysUntilExpiration.Value);

        var batch = Batch.New(userDetails.UserId, userDetails.GetHouseholdId(), cmd.FoodId, cmd.StorageSpaceId,
            cmd.Quantity,
            cmd.FoodUoMId, bestBeforeDate, expirationDate);

        await batchRepository.AddAsync(batch);

        var batchModification = BatchModificationHistory.Initial(batch, userDetails.UserId);

        await batchModificationHistoryRepository.AddAsync(batchModification);

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    /**
     * MoveBatch changes the storage space for some quantity of a batch.
     * 
     * MoveBatchCmd specifies the quantity to move and the new storage space. If new batch quantity is not provided and the unit of the quantity moved and the unit of the batch differ, the quantity moved is converted to the batch unit to check if all quantity is moved. If the conversion is approximate, the moved quantity is always treated as a partial movement, leaving the original batch with quantity 0.
     */
    public async Task<Result<List<BatchDto>>> MoveBatch(int batchId, MoveBatchCmd cmd, AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<List<BatchDto>>(validationError);

        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<List<BatchDto>>(new NotFoundError(EntityType.Batch, batchId));

        var food = await foodRepository.GetFoodById(batch.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<List<BatchDto>>(new NotFoundError(EntityType.Food, batch.FoodId));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.GetHouseholdId()))
            return new Result<List<BatchDto>>(new NotFoundError(EntityType.StorageSpace, cmd.StorageSpaceId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<List<BatchDto>>(new FoodUoMNotFound(batch.FoodId, cmd.FoodUoMId));

        Batch? newBatch = null;

        if (cmd is { NewQuantity: not null, NewFoodUoMId: not null })
        {
            if (food.UoM.All(x => x.Id != cmd.NewFoodUoMId.Value))
                return new Result<List<BatchDto>>(new FoodUoMNotFound(batch.FoodId, cmd.NewFoodUoMId.Value));

            if (cmd.NewQuantity.Value == 0)
            {
                // Full quantity has been moved. Change batch storage space and log batch modifications
                batch.StorageSpaceId = cmd.StorageSpaceId;

                var batchModification =
                    BatchModificationHistory.FullMove(batch, userDetails.UserId, cmd.StorageSpaceId);

                await batchModificationHistoryRepository.AddAsync(batchModification);
            }
            else
            {
                // Partial move. Change batch quantity and create new batch from this batch 
                var previousBatchQuantity = batch.Quantity;
                var previousBatchFoodUoMId = batch.FoodUoMId;

                batch.Quantity = cmd.NewQuantity.Value;
                batch.FoodUoMId = cmd.NewFoodUoMId.Value;

                newBatch = Batch.FromOriginal(batch, userDetails.UserId, cmd.StorageSpaceId, cmd.Quantity,
                    cmd.FoodUoMId, batch.BestBeforeDate, batch.ExpirationDate);

                await batchRepository.AddAsync(newBatch);

                var originalBatchModification = BatchModificationHistory.OriginalBatchPartialMove(batch, newBatch,
                    userDetails.UserId, previousBatchQuantity, previousBatchFoodUoMId);

                await batchModificationHistoryRepository.AddAsync(originalBatchModification);

                var batchModification =
                    BatchModificationHistory.NewBatchPartialMove(newBatch, batch, userDetails.UserId);

                await batchModificationHistoryRepository.AddAsync(batchModification);
            }
        }
        else
        {
            // New batch quantity was not provided, convert quantity to check if move is full move or partial move

            var convertResult = food.Convert(cmd.Quantity, cmd.FoodUoMId, batch.FoodUoMId);
            if (convertResult.Failed()) return new Result<List<BatchDto>>(convertResult.Error());

            var quantityConversion = convertResult.Get();

            if (!quantityConversion.IsApproximate && Math.Abs(quantityConversion.Quantity - batch.Quantity) > 1e-6)
                return new Result<List<BatchDto>>(
                    new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));


            if (!quantityConversion.IsApproximate && Math.Abs(quantityConversion.Quantity - batch.Quantity) < 1e-6)
            {
                // Full move on exact conversion. Change batch storage space and log batch modifications
                batch.StorageSpaceId = cmd.StorageSpaceId;

                var batchModification =
                    BatchModificationHistory.FullMove(batch, userDetails.UserId, cmd.StorageSpaceId);

                await batchModificationHistoryRepository.AddAsync(batchModification);
            }

            // Either partial move on exact conversion, or any move on approximate conversion. Create new batch and change original batch quantity, and log batch modifications

            var previousBatchQuantity = batch.Quantity;
            var previousBatchFoodUoMId = batch.FoodUoMId;

            batch.Quantity -= quantityConversion.Quantity;

            newBatch = Batch.FromOriginal(batch, userDetails.UserId, cmd.StorageSpaceId, cmd.Quantity, cmd.FoodUoMId,
                batch.BestBeforeDate, batch.ExpirationDate);

            await batchRepository.AddAsync(newBatch);

            var originalBatchModification = BatchModificationHistory.OriginalBatchPartialMove(batch, newBatch,
                userDetails.UserId, previousBatchQuantity, previousBatchFoodUoMId);

            await batchModificationHistoryRepository.AddAsync(originalBatchModification);

            var newBatchModification =
                BatchModificationHistory.NewBatchPartialMove(newBatch, batch, userDetails.UserId);

            await batchModificationHistoryRepository.AddAsync(newBatchModification);
        }

        await dbContext.SaveChangesAsync();

        return newBatch == null
            ? new Result<List<BatchDto>>([new BatchDto(batch)])
            : new Result<List<BatchDto>>([new BatchDto(batch), new BatchDto(newBatch)]);
    }

    // public async Task<List<BatchDto>> GetAllBatches(AuthUserDetails userDetails)
    // {
    //     var batches = await batchRepository.GetAllByHouseholdId(userDetails.GetHouseholdId());
    //
    //     return BatchDto.ToBatchDtoList(batches);
    // }
    //
    // public async Task<Result<BatchDto>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails)
    // {
    //     var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
    //     if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));
    //
    //     if (batch.BestBeforeDate != cmd.BestBeforeDate) batch.BestBeforeDate = cmd.BestBeforeDate;
    //
    //     if (batch.ExpirationDate != cmd.ExpirationDate) batch.ExpirationDate = cmd.ExpirationDate;
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return new Result<BatchDto>(new BatchDto(batch));
    // }
    //
    // public async Task<Result<BatchDto>> ConsumeBatch(int batchId, ConsumeBatchCmd cmd, AuthUserDetails userDetails)
    // {
    //     var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
    //     if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));
    //
    //     if (cmd.Quantity > batch.Quantity)
    //         return new Result<BatchDto>(new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));
    //
    //     if (Math.Abs(cmd.Quantity - batch.Quantity) < 1e-6)
    //     {
    //         // Set batch as fully consumed
    //         batch.Consume();
    //     }
    //     else
    //     {
    //         // Create new batch from original batch with new quantity and set new batch as consumed
    //         var consumedBatch = new Batch(batch, cmd.Quantity);
    //         consumedBatch.Consume();
    //
    //         await batchRepository.AddAsync(consumedBatch);
    //
    //         batch.Quantity -= cmd.Quantity;
    //     }
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return new Result<BatchDto>(new BatchDto(batch));
    // }
    //
    // public async Task<Result<BatchDto>> DiscardBatch(int batchId, DiscardBatchCmd cmd, AuthUserDetails userDetails)
    // {
    //     var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
    //     if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));
    //
    //     if (cmd.Quantity > batch.Quantity)
    //         return new Result<BatchDto>(new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));
    //
    //     if (Math.Abs(cmd.Quantity - batch.Quantity) < 1e-6)
    //     {
    //         // Set batch as fully discarded
    //         batch.Discard();
    //     }
    //     else
    //     {
    //         // Create new batch from original batch with new quantity and set new batch as discarded
    //         var consumedBatch = new Batch(batch, cmd.Quantity);
    //         consumedBatch.Discard();
    //
    //         await batchRepository.AddAsync(consumedBatch);
    //
    //         batch.Quantity -= cmd.Quantity;
    //     }
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return new Result<BatchDto>(new BatchDto(batch));
    // }
    //
}