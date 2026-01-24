using Capychef.Persistence;
using Capychef.Storage.Domain.Entities;
using Capychef.Storage.Domain.Interfaces;

namespace Capychef.Storage;

public class BatchRepository(CapychefDbContext dbContext) : IBatchRepository
{
    public async Task AddAsync(Batch batch)
    {
        await dbContext.Batches.AddAsync(batch);
    }
}