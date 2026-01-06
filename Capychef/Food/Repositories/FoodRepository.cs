using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Repositories;

public class FoodRepository(CapychefDbContext dbContext) : IFoodRepository
{
    public async Task AddAsync(Domain.Entities.Food food)
    {
        await dbContext.Food.AddAsync(food);
    }

    public async Task<List<Domain.Entities.Food>> GetTrackedAllGlobalFood()
    {
        return await dbContext.Food.Where(x => x.IsGlobal).ToListAsync();
    }

    public async Task<List<Domain.Entities.Food>> GetAllGlobalFood()
    {
        return await dbContext.Food.AsNoTracking().Where(x => x.IsGlobal).ToListAsync();
    }
}