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
using Capychef.Storage.Domain.Interfaces;

namespace Capychef.Storage.Services;

public class BatchService(
    CapychefDbContext dbContext,
    IBatchRepository batchRepository,
    IFoodRepository foodRepository,
    IStorageSpaceRepository storageSpaceRepository,
    IFoodUoMRepository foodUoMRepository) : IBatchService
{
    public async Task<Result<BatchDTO>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd)
    {
        if (!await foodRepository.CheckFoodExistsById(cmd.FoodId, userDetails.HouseholdId.Value))
            return new Result<BatchDTO>(new NotFoundError(EntityType.Food, cmd.FoodId));

        if (!await storageSpaceRepository.CheckStorageSpaceExistsById(cmd.StorageSpaceId,
                userDetails.HouseholdId.Value))
            return new Result<BatchDTO>(new NotFoundError(EntityType.StorageSpace, cmd.FoodId));

        if (!await foodUoMRepository.CheckFoodUoMExistsById(cmd.FoodUoMId, cmd.FoodId))
            return new Result<BatchDTO>(new NotFoundError(EntityType.FoodUoM,
                $"{cmd.FoodUoMId} for food {cmd.FoodId}"));

        var batch = new Batch(userDetails.HouseholdId.Value, cmd.FoodId, cmd.StorageSpaceId, cmd.Quantity,
            cmd.FoodUoMId);

        await batchRepository.AddAsync(batch);

        await dbContext.SaveChangesAsync();

        return new Result<BatchDTO>(new BatchDTO(batch));
    }
}