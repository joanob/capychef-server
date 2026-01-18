using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Households.Repositories;

public class StorageSpaceRepository(CapychefDbContext dbContext) : IStorageSpaceRepository
{
    public async Task AddAsync(StorageSpace storageSpace)
    {
        await dbContext.StorageSpaces.AddAsync(storageSpace);
    }
}