using Capychef.Persistence;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Shopping.Repositories;

public class SupermarketFoodDetailsRepository(CapychefDbContext dbContext) : ISupermarketFoodDetailsRepository
{
    public async Task AddAsync(SupermarketFoodDetails details)
    {
        await dbContext.SupermarketFoodDetails.AddAsync(details);
    }

    public async Task<SupermarketFoodDetails?> FindTrackedById(int id, int householdId)
    {
        return await dbContext.SupermarketFoodDetails
            .Where(x => !x.IsDeleted && x.HouseholdId == householdId && x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<SupermarketFoodDetails?> FindTrackedByFoodAndSupermarket(int foodId, int supermarketId,
        int householdId)
    {
        return await dbContext.SupermarketFoodDetails
            .Where(x => !x.IsDeleted
                        && x.FoodId == foodId
                        && x.SupermarketId == supermarketId
                        && x.HouseholdId == householdId)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(SupermarketFoodDetails details)
    {
        dbContext.SupermarketFoodDetails.Remove(details);
        await Task.CompletedTask;
    }
}