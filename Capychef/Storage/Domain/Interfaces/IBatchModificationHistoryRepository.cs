using Capychef.Storage.Domain.Entities;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchModificationHistoryRepository
{
    Task AddAsync(BatchModificationHistory batchModification);
}