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

    public async Task<List<Domain.Entities.Food>> GetTrackedAllGlobalFood(int? householdId)
    {
        return await dbContext.Food.Active().IncludeUoM(householdId).Where(x => x.IsGlobal).ToListAsync();
    }

    public async Task<List<Domain.Entities.Food>> GetAllGlobalFood(int? householdId)
    {
        return await dbContext.Food.AsNoTracking().Active().IncludeUoM(householdId).Where(x => x.IsGlobal)
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Food>> GetAllHouseholdFood(int householdId)
    {
        var food = await dbContext.Food.AsNoTracking().Active().IncludeUoM(householdId)
            .Where(x => x.HouseholdId == householdId)
            .ToListAsync();

        foreach (var item in food) item.FromHousehold(householdId);

        return food;
    }

    public async Task<Domain.Entities.Food?> GetTrackedHouseholdFoodById(int foodId, int householdId)
    {
        var food = await dbContext.Food.Active().IncludeUoM(householdId)
            .FirstOrDefaultAsync(x => x.Id == foodId && x.HouseholdId == householdId);

        food.FromHousehold(householdId);

        return food;
    }

    public async Task<bool> CheckFoodExistsById(int cmdFoodId, int householdId)
    {
        return await dbContext.Food.Active()
            .AnyAsync(x => x.Id == cmdFoodId && (x.IsGlobal || x.HouseholdId == householdId));
    }

    public async Task<Domain.Entities.Food?> GetFoodById(int id, int householdId)
    {
        var food = await dbContext.Food.AsNoTracking().Active().IncludeUoM(householdId)
            .FirstOrDefaultAsync(x => x.Id == id && (x.IsGlobal || x.HouseholdId == householdId));

        food.FromHousehold(householdId);

        return food;
    }

    public async Task<Domain.Entities.Food?> GetTrackedFoodById(int id, int householdId)
    {
        var food = await dbContext.Food.Active().IncludeUoM(householdId)
            .FirstOrDefaultAsync(x => x.Id == id && (x.IsGlobal || x.HouseholdId == householdId));

        food.FromHousehold(householdId);

        return food;
    }

    public async Task<Domain.Entities.Food?> GetGlobalFoodById(int foodId, int? householdId)
    {
        return await dbContext.Food.AsNoTracking().Active().IncludeUoM(householdId)
            .FirstOrDefaultAsync(x => x.Id == foodId && x.IsGlobal);
    }

    public async Task<Domain.Entities.Food?> GetTrackedGlobalFoodById(int foodId, int? householdId)
    {
        return await dbContext.Food.Active().IncludeUoM(householdId)
            .FirstOrDefaultAsync(x => x.Id == foodId && x.IsGlobal);
    }
}