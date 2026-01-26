using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IStorageSpaceModificationHistoryRepository
{
    Task AddAsync(StorageSpacesModificationHistory storageSpacesModificationHistory);
}