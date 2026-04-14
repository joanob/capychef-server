using Capychef.Persistence;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Shopping.Repositories;

public class SupermarketRepository(CapychefDbContext dbContext) : ISupermarketRepository
{
    public async Task AddAsync(Supermarket supermarket)
    {
        await dbContext.Supermarkets.AddAsync(supermarket);
    }

    public async Task<Supermarket?> FindTrackedById(int id)
    {
        return await dbContext.Supermarkets.Active().FirstOrDefaultAsync(x => x.Id == id);
    }
}