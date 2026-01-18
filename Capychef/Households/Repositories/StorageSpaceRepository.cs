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
}