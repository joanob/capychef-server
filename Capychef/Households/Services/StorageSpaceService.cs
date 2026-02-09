using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Households.Services;

public class StorageSpaceService(
    CapychefDbContext dbContext,
    IStorageSpaceRepository storageSpaceRepository,
    IStorageSpaceModificationHistoryRepository storageSpaceModificationHistoryRepository)
    : IStorageSpaceService
{
    public async Task<Result<StorageSpaceDTO>> CreateStorageSpace(CreateStorageSpaceCmd cmd,
        AuthUserDetails userDetails)
    {
        var storageCondition = StorageConditions.from(cmd.StorageCondition);

        if (storageCondition == null)
            return new Result<StorageSpaceDTO>(new NotFoundError(EntityType.StorageCondition, cmd.StorageCondition));

        var storageSpace =
            new StorageSpace(cmd.Name, storageCondition, userDetails.HouseholdId.Value, userDetails.UserId);

        await storageSpaceRepository.AddAsync(storageSpace);

        await dbContext.SaveChangesAsync();

        return new Result<StorageSpaceDTO>(new StorageSpaceDTO(storageSpace));
    }

    public async Task<Result<StorageSpaceDTO>> UpdateStorageSpace(int id, UpdateStorageSpaceCmd cmd,
        AuthUserDetails userDetails)
    {
        var storageSpace = await storageSpaceRepository.GetTrackedStorageSpaceById(id, userDetails.HouseholdId.Value);

        if (storageSpace == null) return new Result<StorageSpaceDTO>(new NotFoundError(EntityType.StorageSpace, id));

        if (storageSpace.Name != cmd.Name)
        {
            await storageSpaceModificationHistoryRepository.AddAsync(new StorageSpacesModificationHistory(
                storageSpace.Id, StorageSpaceModifiableColumn.Name, storageSpace.Name, cmd.Name, userDetails.UserId));

            storageSpace.Name = cmd.Name;
        }

        if (storageSpace.StorageCondition.ToString() != cmd.StorageCondition)
        {
            var storageCondition = StorageConditions.from(cmd.StorageCondition);

            if (storageCondition == null)
                return new Result<StorageSpaceDTO>(
                    new NotFoundError(EntityType.StorageCondition, cmd.StorageCondition));

            await storageSpaceModificationHistoryRepository.AddAsync(new StorageSpacesModificationHistory(
                storageSpace.Id, StorageSpaceModifiableColumn.StorageCondition,
                storageSpace.StorageCondition.ToString(), cmd.StorageCondition, userDetails.UserId));

            storageSpace.StorageCondition = storageCondition;
        }

        await dbContext.SaveChangesAsync();

        return new Result<StorageSpaceDTO>(new StorageSpaceDTO(storageSpace));
    }

    public async Task<AppError> DeleteStorageSpace(int id, AuthUserDetails userDetails)
    {
        var storageSpace = await storageSpaceRepository.GetTrackedStorageSpaceById(id, userDetails.HouseholdId.Value);

        if (storageSpace == null) return new NotFoundError(EntityType.StorageSpace, id);

        storageSpace.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<List<StorageSpaceDTO>>> GetHouseholdStorageSpaces(AuthUserDetails userDetails)
    {
        var storageSpaces = await storageSpaceRepository.GetHouseholdStorageSpaces(userDetails.HouseholdId.Value);

        return new Result<List<StorageSpaceDTO>>(StorageSpaceDTO.ToDTOList(storageSpaces));
    }
}