using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IStorageSpaceRepository
{
    public Task AddAsync(StorageSpace storageSpace);
}