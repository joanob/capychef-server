using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
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
    IFoodRepository foodRepository,
    IStorageSpaceRepository storageSpaceRepository,
    IFoodUoMRepository foodUoMRepository) : IBatchService
{
    public async Task<Result<BatchDto>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(cmd.FoodId, userDetails.GetHouseholdId());
        if (food == null) return new Result<BatchDto>(new NotFoundError(EntityType.Food, cmd.FoodId));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.GetHouseholdId()))
            return new Result<BatchDto>(new NotFoundError(EntityType.StorageSpace, cmd.FoodId));

        if (!await foodUoMRepository.CheckFoodUoMExistsById(cmd.FoodUoMId, cmd.FoodId, userDetails.GetHouseholdId()))
            return new Result<BatchDto>(new NotFoundError(EntityType.FoodUoM,
                $"{cmd.FoodUoMId} for food {cmd.FoodId}"));

        DateTime? bestBeforeDate = null;

        if (cmd.BestBeforeDate.HasValue)
            bestBeforeDate = cmd.BestBeforeDate.Value;
        else if (food.HouseholdFoodDetails != null && food.HouseholdFoodDetails.DaysUntilBestBefore.HasValue)
            bestBeforeDate = DateTime.UtcNow.AddDays(food.HouseholdFoodDetails.DaysUntilBestBefore.Value);

        DateTime? expirationDate = null;

        if (cmd.ExpirationDate.HasValue)
            expirationDate = cmd.ExpirationDate.Value;
        else if (food.HouseholdFoodDetails != null && food.HouseholdFoodDetails.DaysUntilExpiration.HasValue)
            expirationDate = DateTime.UtcNow.AddDays(food.HouseholdFoodDetails.DaysUntilExpiration.Value);

        var batch = Batch.New(userDetails.GetHouseholdId(), cmd.FoodId, cmd.StorageSpaceId, cmd.Quantity,
            cmd.FoodUoMId, bestBeforeDate, expirationDate);

        await batchRepository.AddAsync(batch);

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    public async Task<List<BatchDto>> GetAllBatches(AuthUserDetails userDetails)
    {
        var batches = await batchRepository.GetAllByHouseholdId(userDetails.GetHouseholdId());

        return BatchDto.ToBatchDtoList(batches);
    }

    public async Task<Result<BatchDto>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails)
    {
        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));

        if (batch.BestBeforeDate != cmd.BestBeforeDate) batch.BestBeforeDate = cmd.BestBeforeDate;

        if (batch.ExpirationDate != cmd.ExpirationDate) batch.ExpirationDate = cmd.ExpirationDate;

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    public async Task<Result<BatchDto>> ConsumeBatch(int batchId, ConsumeBatchCmd cmd, AuthUserDetails userDetails)
    {
        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));

        if (cmd.Quantity > batch.Quantity)
            return new Result<BatchDto>(new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));

        if (Math.Abs(cmd.Quantity - batch.Quantity) < 1e-6)
        {
            // Set batch as fully consumed
            batch.Consume();
        }
        else
        {
            // Create new batch from original batch with new quantity and set new batch as consumed
            var consumedBatch = Batch.FromOriginal(batch, cmd.Quantity);
            consumedBatch.Consume();

            await batchRepository.AddAsync(consumedBatch);

            batch.Quantity -= cmd.Quantity;
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    public async Task<Result<BatchDto>> DiscardBatch(int batchId, DiscardBatchCmd cmd, AuthUserDetails userDetails)
    {
        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<BatchDto>(new NotFoundError(EntityType.Batch, batchId));

        if (cmd.Quantity > batch.Quantity)
            return new Result<BatchDto>(new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));

        if (Math.Abs(cmd.Quantity - batch.Quantity) < 1e-6)
        {
            // Set batch as fully discarded
            batch.Discard();
        }
        else
        {
            // Create new batch from original batch with new quantity and set new batch as discarded
            var consumedBatch = Batch.FromOriginal(batch, cmd.Quantity);
            consumedBatch.Discard();

            await batchRepository.AddAsync(consumedBatch);

            batch.Quantity -= cmd.Quantity;
        }

        await dbContext.SaveChangesAsync();

        return new Result<BatchDto>(new BatchDto(batch));
    }

    public async Task<Result<List<BatchDto>>> MoveBatch(int batchId, MoveBatchCmd cmd, AuthUserDetails userDetails)
    {
        var batch = await batchRepository.GetTrackedBatchById(batchId, userDetails.GetHouseholdId());
        if (batch == null) return new Result<List<BatchDto>>(new NotFoundError(EntityType.Batch, batchId));

        if (cmd.Quantity > batch.Quantity)
            return new Result<List<BatchDto>>(
                new BatchDoesNotHaveEnoughQuantityError(batchId, batch.Quantity, cmd.Quantity));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.GetHouseholdId()))
            return new Result<List<BatchDto>>(new NotFoundError(EntityType.StorageSpace, cmd.StorageSpaceId));

        if (Math.Abs(cmd.Quantity - batch.Quantity) < 1e-6)
        {
            // Change batch storage space
            batch.StorageSpaceId = cmd.StorageSpaceId;

            await dbContext.SaveChangesAsync();

            return new Result<List<BatchDto>>(new List<BatchDto> { new(batch) });
        }

        // Create new batch from original batch with new quantity and new storage space
        var movedBatch = Batch.FromOriginal(batch, cmd.Quantity);
        movedBatch.StorageSpaceId = cmd.StorageSpaceId;

        await batchRepository.AddAsync(movedBatch);

        batch.Quantity -= cmd.Quantity;

        await dbContext.SaveChangesAsync();

        return new Result<List<BatchDto>>(new List<BatchDto> { new(batch), new(movedBatch) });
    }
}