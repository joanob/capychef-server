using Capychef.Persistence;
using Capychef.Storage.Domain.Entities;
using Capychef.Storage.Domain.Interfaces;

namespace Capychef.Storage.Repositories;

public class BatchModificationHistoryRepository(CapychefDbContext dbContext) : IBatchModificationHistoryRepository
{
    public async Task AddAsync(BatchModificationHistory batchModification)
    {
        await dbContext.BatchesModificationHistory.AddAsync(batchModification);
    }
}