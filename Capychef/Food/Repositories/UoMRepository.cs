using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Repositories;

public class UoMRepository(CapychefDbContext dbContext) : IUoMRepository
{
    public async Task AddAsync(UoM uom)
    {
        await dbContext.UoM.AddAsync(uom);
    }
}