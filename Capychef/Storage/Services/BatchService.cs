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
    public async Task<Result<BatchModificationDto>> MoveBatch(int batchId, MoveBatchCmd cmd,
        AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<BatchModificationDto>(validationError);

        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Batch, batchId));

        var food = await foodRepository.GetFoodById(batch.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Food, batch.FoodId));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.GetHouseholdId()))
            return new Result<BatchModificationDto>(new NotFoundError(EntityType.StorageSpace, cmd.StorageSpaceId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.FoodUoMId));

        // Validate NewFoodUoMId if provided
        if (cmd is { NewFoodUoMId: not null } && food.UoM.All(x => x.Id != cmd.NewFoodUoMId.Value))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.NewFoodUoMId.Value));

        var modificationResult = await PerformBatchModification(batch, cmd, food, userDetails.UserId);
        if (modificationResult.Failed()) return new Result<BatchModificationDto>(modificationResult.Error());

        var batchModifications = modificationResult.Get();

        if (batchModifications.NewBatch == null)
        {
            // Full move: no new batch was created

            batchModifications.OriginalBatch.StorageSpaceId = cmd.StorageSpaceId;

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.FullMove(batchModifications.OriginalBatch, userDetails.UserId,
                    cmd.StorageSpaceId));
        }
        else
        {
            // Partial move: new batch was created

            batchModifications.NewBatch.StorageSpaceId = cmd.StorageSpaceId;

            await batchRepository.AddAsync(batchModifications.NewBatch);

            await batchModificationHistoryRepository.AddAsync(BatchModificationHistory.OriginalBatchPartialMove(
                batchModifications.OriginalBatch, batchModifications.NewBatch, userDetails.UserId,
                batchModifications.PreviousBatchQuantity, batchModifications.PreviousBatchFoodUoMId));

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.NewBatchPartialMove(batchModifications.NewBatch,
                    batchModifications.OriginalBatch, userDetails.UserId));
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchModificationDto>(new BatchModificationDto(batchModifications.OriginalBatch,
            batchModifications.NewBatch));
    }

    /// <summary>
    ///     ConsumeBatch marks some quantity of a batch as consumed.
    ///     If the entire batch quantity is consumed, only the original batch is marked as consumed.
    ///     If only part of the batch is consumed, a new batch is created with the consumed quantity and marked as consumed.
    /// </summary>
    public async Task<Result<BatchModificationDto>> ConsumeBatch(int batchId, ModifyBatchCmd cmd,
        AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<BatchModificationDto>(validationError);

        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Batch, batchId));

        var food = await foodRepository.GetFoodById(batch.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Food, batch.FoodId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.FoodUoMId));

        // Validate NewFoodUoMId if provided
        if (cmd is { NewFoodUoMId: not null } && food.UoM.All(x => x.Id != cmd.NewFoodUoMId.Value))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.NewFoodUoMId.Value));

        var modificationResult = await PerformBatchModification(batch, cmd, food, userDetails.UserId);
        if (modificationResult.Failed()) return new Result<BatchModificationDto>(modificationResult.Error());

        var batchModifications = modificationResult.Get();

        if (batchModifications.NewBatch == null)
        {
            // Full consume: no new batch was created
            batchModifications.OriginalBatch.Consume();

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.FullConsume(batchModifications.OriginalBatch, userDetails.UserId));
        }
        else
        {
            // Partial consume: new batch was created and marked as consumed
            batchModifications.NewBatch.Consume();

            await batchRepository.AddAsync(batchModifications.NewBatch);

            await batchModificationHistoryRepository.AddAsync(BatchModificationHistory.OriginalBatchPartialConsume(
                batchModifications.OriginalBatch, batchModifications.NewBatch, userDetails.UserId,
                batchModifications.PreviousBatchQuantity, batchModifications.PreviousBatchFoodUoMId));

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.ConsumedBatchPartialConsume(batchModifications.NewBatch,
                    batchModifications.OriginalBatch, userDetails.UserId));
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchModificationDto>(new BatchModificationDto(batchModifications.OriginalBatch,
            batchModifications.NewBatch));
    }

    /// <summary>
    ///     DiscardBatch marks some quantity of a batch as discarded.
    ///     If the entire batch quantity is discarded, only the original batch is marked as discarded.
    ///     If only part of the batch is discarded, a new batch is created with the discarded quantity and marked as discarded.
    /// </summary>
    public async Task<Result<BatchModificationDto>> DiscardBatch(int batchId, ModifyBatchCmd cmd,
        AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<BatchModificationDto>(validationError);

        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Batch, batchId));

        var food = await foodRepository.GetFoodById(batch.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchModificationDto>(new NotFoundError(EntityType.Food, batch.FoodId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.FoodUoMId));

        // Validate NewFoodUoMId if provided
        if (cmd is { NewFoodUoMId: not null } && food.UoM.All(x => x.Id != cmd.NewFoodUoMId.Value))
            return new Result<BatchModificationDto>(new FoodUoMNotFound(batch.FoodId, cmd.NewFoodUoMId.Value));

        var modificationResult = await PerformBatchModification(batch, cmd, food, userDetails.UserId);
        if (modificationResult.Failed()) return new Result<BatchModificationDto>(modificationResult.Error());

        var batchModifications = modificationResult.Get();

        if (batchModifications.NewBatch == null)
        {
            // Full discard: no new batch was created
            batchModifications.OriginalBatch.Discard();

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.FullDiscard(batchModifications.OriginalBatch, userDetails.UserId));
        }
        else
        {
            // Partial discard: new batch was created and marked as discarded
            batchModifications.NewBatch.Discard();

            await batchRepository.AddAsync(batchModifications.NewBatch);

            await batchModificationHistoryRepository.AddAsync(BatchModificationHistory.OriginalBatchPartialDiscard(
                batchModifications.OriginalBatch, batchModifications.NewBatch, userDetails.UserId,
                batchModifications.PreviousBatchQuantity, batchModifications.PreviousBatchFoodUoMId));

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.DiscardedBatchPartialDiscard(batchModifications.NewBatch,
                    batchModifications.OriginalBatch, userDetails.UserId));
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchModificationDto>(new BatchModificationDto(batchModifications.OriginalBatch,
            batchModifications.NewBatch));
    }

    public async Task<Result<BatchDto>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<BatchDto>(validationError);

        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));

        var food = await foodRepository.GetFoodById(batch.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchDto>(new NotFoundError(EntityType.Food, batch.FoodId));

        if (food.UoM.All(x => x.Id != cmd.FoodUoMId))
            return new Result<BatchDto>(new FoodUoMNotFound(batch.FoodId, cmd.FoodUoMId));

        if (batch.Quantity - cmd.Quantity > -1e-6 || batch.FoodUoMId != cmd.FoodUoMId)
        {
            var previousQuantity = batch.Quantity;
            var previousFoodUoMId = batch.FoodUoMId;

            batch.Quantity = cmd.Quantity;
            batch.FoodUoMId = cmd.FoodUoMId;

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.UpdateQuantity(batch, userDetails.UserId, previousQuantity,
                    previousFoodUoMId));
        }

        if (batch.BestBeforeDate != cmd.BestBeforeDate)
        {
            batch.BestBeforeDate = cmd.BestBeforeDate;

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.UpdateBestBeforeDate(batch, userDetails.UserId));
        }

        if (batch.ExpirationDate != cmd.ExpirationDate)
        {
            batch.ExpirationDate = cmd.ExpirationDate;

            await batchModificationHistoryRepository.AddAsync(
                BatchModificationHistory.UpdateExpirationDate(batch, userDetails.UserId));
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    public async Task<List<BatchDto>> GetAllBatches(AuthUserDetails userDetails)
    {
        var batches = await batchRepository.GetAllByHouseholdId(userDetails.GetHouseholdId());

        return BatchDto.ToBatchDtoList(batches);
    }

    /// <summary>
    ///     Generic batch modification method that handles conversion, quantity checks, and batch creation/modification.
    ///     This method contains the common logic for Move, Consume, and Discard operations.
    ///     Logic:
    ///     - If NewQuantity and NewFoodUoMId are provided: uses them directly without conversion
    ///     - Otherwise: converts the provided quantity to the batch's unit to check if it's a full or partial move
    ///     - If conversion is approximate but results in remainder: treats as partial move (doesn't delete original batch)
    ///     - If conversion is exact and equals batch quantity: it's a full move
    ///     - Otherwise: it's a partial move with a new batch created
    /// </summary>
    /// <param name="batch">The batch to modify (tracked by EF Core)</param>
    /// <param name="cmd">The modification command containing quantity, units, and optional new quantity/unit</param>
    /// <param name="food">The food entity for unit conversions</param>
    /// <param name="userId">User ID for tracking modifications</param>
    /// <returns>Result containing the modification details (original batch, new batch if created, previous values)</returns>
    private async Task<Result<GenericModificationResult>> PerformBatchModification(
        Batch batch,
        ModifyBatchCmd cmd,
        Food.Domain.Entities.Food food,
        int userId)
    {
        Batch? newBatch = null;
        var previousBatchQuantity = batch.Quantity;
        var previousBatchFoodUoMId = batch.FoodUoMId;

        if (cmd is { NewQuantity: not null, NewFoodUoMId: not null })
        {
            // New batch quantity and unit were provided explicitly
            if (cmd.NewQuantity.Value != 0)
            {
                batch.Quantity = cmd.NewQuantity.Value;
                batch.FoodUoMId = cmd.NewFoodUoMId.Value;

                newBatch = Batch.FromOriginal(batch, userId, batch.StorageSpaceId, cmd.Quantity,
                    cmd.FoodUoMId, batch.BestBeforeDate, batch.ExpirationDate);

                await batchRepository.AddAsync(newBatch);
            }
        }
        else
        {
            // New batch quantity was not provided, convert quantity to check if move is full or partial

            var convertResult = food.Convert(cmd.Quantity, cmd.FoodUoMId, batch.FoodUoMId);
            if (convertResult.Failed()) return new Result<GenericModificationResult>(convertResult.Error());

            var quantityConversion = convertResult.Get();

            if (!quantityConversion.IsApproximate && Math.Abs(quantityConversion.Quantity - batch.Quantity) > 1e-6)
                return new Result<GenericModificationResult>(
                    new BatchDoesNotHaveEnoughQuantityError(batch.Id, batch.Quantity, cmd.Quantity));

            if (!quantityConversion.IsApproximate && Math.Abs(quantityConversion.Quantity - batch.Quantity) < 1e-6)
            {
            }
            else
            {
                batch.Quantity -= quantityConversion.Quantity;

                newBatch = Batch.FromOriginal(batch, userId, batch.StorageSpaceId, cmd.Quantity, cmd.FoodUoMId,
                    batch.BestBeforeDate, batch.ExpirationDate);

                await batchRepository.AddAsync(newBatch);
            }
        }

        return new Result<GenericModificationResult>(
            new GenericModificationResult(batch, newBatch, previousBatchQuantity, previousBatchFoodUoMId));
    }
}

/// <summary>
///     Represents the result of a batch modification operation during the generic phase.
///     Contains information needed by the calling method to decide what additional modifications to apply.
/// </summary>
internal record GenericModificationResult(
    Batch OriginalBatch,
    Batch? NewBatch,
    double PreviousBatchQuantity,
    int PreviousBatchFoodUoMId);