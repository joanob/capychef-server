using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Repositories;

public class StorageSpaceRepository(CapychefDbContext dbContext) : IStorageSpaceRepository
{
    public async Task AddAsync(StorageSpace storageSpace)
    {
        await dbContext.StorageSpaces.AddAsync(storageSpace);
    }

    public async Task<StorageSpace?> GetTrackedStorageSpaceById(int id, int householdId)
    {
        return await dbContext.StorageSpaces.Active()
            .FirstOrDefaultAsync(x => x.Id == id && x.HouseholdId == householdId);
    }

    public async Task<bool> CheckStorageSpaceExistsById(int cmdStorageSpaceId, int householdId)
    {
        return await dbContext.StorageSpaces.Active()
            .AnyAsync(x => x.Id == cmdStorageSpaceId && x.HouseholdId == householdId);
    }

    public async Task<List<StorageSpace>> GetHouseholdStorageSpaces(int householdId)
    {
        return await dbContext.StorageSpaces.Active().AsNoTracking().Where(x => x.HouseholdId == householdId)
            .ToListAsync();
    }

    public async Task<List<InitialStorageSpace>> GetInitialStorageSpaces()
    {
        return await dbContext.InitialStorageSpaces.AsNoTracking().ToListAsync();
    }
}