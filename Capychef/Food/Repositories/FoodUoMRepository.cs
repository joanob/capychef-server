using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Repositories;

public class FoodUoMRepository(CapychefDbContext dbContext) : IFoodUoMRepository
{
    public async Task AddAsync(FoodUoM foodUoM)
    {
        await dbContext.FoodUoM.AddAsync(foodUoM);
    }

    public async Task<List<FoodUoM>> GetTrackedAllUoMByFoodId(int foodId)
    {
        return await dbContext.FoodUoM.Where(x => x.FoodId == foodId).ToListAsync();
    }

    public async Task<bool> CheckFoodUoMExistsById(int foodUoMId, int foodId)
    {
        return await dbContext.FoodUoM.AnyAsync(x => x.FoodId == foodUoMId && x.FoodId == foodId);
    }
}