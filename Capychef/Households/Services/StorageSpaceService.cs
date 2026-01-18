using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Services;

public class StorageSpaceService(CapychefDbContext dbContext, IStorageSpaceRepository storageSpaceRepository)
    : IStorageSpaceService
{
    public async Task<Result<StorageSpaceDTO>> CreateStorageSpace(CreateStorageSpaceCmd cmd,
        AuthUserDetails userDetails)
    {
        var storageCondition = StorageConditions.from(cmd.StorageConditions);

        if (storageCondition == null)
            return new Result<StorageSpaceDTO>(new NotFoundError(EntityType.StorageCondition, cmd.StorageConditions));

        var storageSpace =
            new StorageSpace(cmd.Name, storageCondition, userDetails.HouseholdId.Value, userDetails.UserId);

        await storageSpaceRepository.AddAsync(storageSpace);

        await dbContext.SaveChangesAsync();

        return new Result<StorageSpaceDTO>(new StorageSpaceDTO(storageSpace));
    }
}