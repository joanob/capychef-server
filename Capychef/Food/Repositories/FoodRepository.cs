using Capychef.Food.Domain.Entities;
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
        return await dbContext.Food.Active().IncludeUoM().Where(x => x.IsGlobal).ToListAsync();
    }

    public async Task<List<Domain.Entities.Food>> GetAllGlobalFood()
    {
        return await dbContext.Food.AsNoTracking().Active().IncludeUoM().Where(x => x.IsGlobal).ToListAsync();
    }

    public async Task<List<Domain.Entities.Food>> GetAllHouseholdFood(int householdId)
    {
        return await dbContext.Food.AsNoTracking().Active().IncludeUoM().Where(x => x.HouseholdId == householdId)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Food> GetTrackedHouseholdFoodById(int foodId, int householdId)
    {
        return await dbContext.Food.Active().IncludeUoM()
            .FirstOrDefaultAsync(x => x.Id == foodId && x.HouseholdId == householdId);
    }
}