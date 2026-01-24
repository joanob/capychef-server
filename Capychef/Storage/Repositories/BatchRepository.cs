using Capychef.Persistence;
using Capychef.Storage.Domain.Entities;
using Capychef.Storage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Storage;

public class BatchRepository(CapychefDbContext dbContext) : IBatchRepository
{
    public async Task AddAsync(Batch batch)
    {
        await dbContext.Batches.AddAsync(batch);
    }

    public async Task<List<Batch>> GetAllByHouseholdId(int householdId)
    {
        return await dbContext.Batches.Where(x => x.HouseholdId == householdId).ToListAsync();
    }
}