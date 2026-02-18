using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Repositories;

public class UoMRepository(CapychefDbContext dbContext) : IUoMRepository
{
    public async Task AddAsync(UoM uom)
    {
        await dbContext.UoM.AddAsync(uom);
    }

    public async Task<bool> CheckUoMExists(string uom)
    {
        return await dbContext.UoM.AnyAsync(x => x.Code == uom);
    }

    public async Task<List<UoM>> GetAllUoM()
    {
        return await dbContext.UoM.ToListAsync();
    }
}