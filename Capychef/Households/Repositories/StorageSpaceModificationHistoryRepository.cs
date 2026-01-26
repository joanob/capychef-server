using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Households.Repositories;

public class StorageSpaceModificationHistoryRepository(CapychefDbContext dbContext)
    : IStorageSpaceModificationHistoryRepository
{
    public async Task AddAsync(StorageSpacesModificationHistory storageSpacesModificationHistory)
    {
        await dbContext.StorageSpacesModificationsHistory.AddAsync(storageSpacesModificationHistory);
    }
}