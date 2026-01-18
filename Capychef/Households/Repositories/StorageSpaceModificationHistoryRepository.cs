using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
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