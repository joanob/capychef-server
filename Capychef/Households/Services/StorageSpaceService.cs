using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Services;

public class StorageSpaceService(
    CapychefDbContext dbContext,
    IStorageSpaceRepository storageSpaceRepository,
    IStorageSpaceModificationHistoryRepository storageSpaceModificationHistoryRepository)
    : IStorageSpaceService
{
    public async Task<Result<StorageSpaceDto>> CreateStorageSpace(StorageSpaceCmd cmd,
        AuthUserDetails userDetails)
    {
        var storageCondition = StorageConditions.From(cmd.StorageCondition);

        if (string.IsNullOrEmpty(storageCondition.ToString()))
            return new Result<StorageSpaceDto>(new NotFoundError(EntityType.StorageCondition, cmd.StorageCondition));

        var storageSpace =
            new StorageSpace(cmd.Name, storageCondition, userDetails.GetHouseholdId(), userDetails.UserId);

        await storageSpaceRepository.AddAsync(storageSpace);

        await dbContext.SaveChangesAsync();

        return new Result<StorageSpaceDto>(new StorageSpaceDto(storageSpace));
    }

    public async Task<Result<StorageSpaceDto>> UpdateStorageSpace(int id, StorageSpaceCmd cmd,
        AuthUserDetails userDetails)
    {
        var storageSpace = await storageSpaceRepository.GetTrackedStorageSpaceById(id, userDetails.GetHouseholdId());

        if (storageSpace == null) return new Result<StorageSpaceDto>(new NotFoundError(EntityType.StorageSpace, id));

        if (storageSpace.RowVersion != cmd.RowVersion)
            return new Result<StorageSpaceDto>(new ConcurrencyError());

        if (storageSpace.Name != cmd.Name)
        {
            await storageSpaceModificationHistoryRepository.AddAsync(new StorageSpacesModificationHistory(
                storageSpace.Id, StorageSpaceModifiableColumn.Name, storageSpace.Name, cmd.Name, userDetails.UserId));

            storageSpace.Name = cmd.Name;
        }

        if (storageSpace.StorageCondition.ToString() != cmd.StorageCondition)
        {
            var storageCondition = StorageConditions.From(cmd.StorageCondition);

            if (string.IsNullOrEmpty(storageCondition.ToString()))
                return new Result<StorageSpaceDto>(
                    new NotFoundError(EntityType.StorageCondition, cmd.StorageCondition));

            await storageSpaceModificationHistoryRepository.AddAsync(new StorageSpacesModificationHistory(
                storageSpace.Id, StorageSpaceModifiableColumn.StorageCondition,
                storageSpace.StorageCondition.ToString(), cmd.StorageCondition, userDetails.UserId));

            storageSpace.StorageCondition = storageCondition;
        }

        storageSpace.RowVersion++;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new Result<StorageSpaceDto>(new ConcurrencyError());
        }

        return new Result<StorageSpaceDto>(new StorageSpaceDto(storageSpace));
    }

    public async Task<AppError?> DeleteStorageSpace(int id, AuthUserDetails userDetails)
    {
        var storageSpace = await storageSpaceRepository.GetTrackedStorageSpaceById(id, userDetails.GetHouseholdId());

        if (storageSpace == null) return new NotFoundError(EntityType.StorageSpace, id);

        storageSpace.Delete();

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        return null;
    }

    public async Task<Result<List<StorageSpaceDto>>> GetHouseholdStorageSpaces(AuthUserDetails userDetails)
    {
        var storageSpaces = await storageSpaceRepository.GetHouseholdStorageSpaces(userDetails.GetHouseholdId());

        return new Result<List<StorageSpaceDto>>(StorageSpaceDto.ToDtoList(storageSpaces));
    }

    public async Task<Result<List<InitialStorageSpaceDto>>> GetInitialStorageSpaces()
    {
        var items = await storageSpaceRepository.GetInitialStorageSpaces();
        return new Result<List<InitialStorageSpaceDto>>(InitialStorageSpaceDto.ToDtoList(items));
    }
}