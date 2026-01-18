using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IStorageSpaceModificationHistoryRepository
{
    Task AddAsync(StorageSpacesModificationHistory storageSpacesModificationHistory);
}