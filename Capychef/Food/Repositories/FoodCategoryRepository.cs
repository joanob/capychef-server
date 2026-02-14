using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Repositories;

public class FoodCategoryRepository(CapychefDbContext dbContext) : IFoodCategoryRepository
{
    public async Task<List<FoodCategory>> GetTrackedAllCategories()
    {
        return await dbContext.FoodCategories.ToListAsync();
    }

    public async Task<List<FoodCategory>> GetAllCategories()
    {
        return await dbContext.FoodCategories.AsNoTracking().ToListAsync();
    }

    public async Task<bool> CheckCategoryExistsById(int id)
    {
        return await dbContext.FoodCategories.AnyAsync(x => x.Id == id);
    }

    public async Task<FoodCategory?> GetTrackedCategoryById(int cmdCategoryId)
    {
        return await dbContext.FoodCategories.FirstOrDefaultAsync(x => x.Id == cmdCategoryId);
    }
}