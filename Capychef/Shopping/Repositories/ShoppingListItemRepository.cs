using Capychef.Persistence;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Shopping.Repositories;

public class ShoppingListItemRepository(CapychefDbContext dbContext) : IShoppingListItemRepository
{
    public async Task AddAsync(ShoppingListItem item)
    {
        await dbContext.ShoppingListItems.AddAsync(item);
    }

    public async Task<ShoppingListItem?> FindTrackedById(int id, int householdId)
    {
        return await dbContext.ShoppingListItems
            .Where(x => !x.IsDeleted && x.HouseholdId == householdId && x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<SupermarketFoodDetails?> FindFirstSupermarketFoodDetailsByFoodId(int foodId, int householdId,
        int? foodUoMId)
    {
        var query = dbContext.SupermarketFoodDetails
            .Where(x => !x.IsDeleted && x.FoodId == foodId && x.HouseholdId == householdId);

        if (foodUoMId.HasValue)
            query = query.Where(x => x.FoodUoMId == foodUoMId);

        query = query.OrderBy(x => x.IsPrefferedSupermarket ? 0 : 1);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int?> GetPreferredSupermarketId(int foodId, int? foodUoMId, int householdId)
    {
        var query = dbContext.SupermarketFoodDetails
            .Where(x => !x.IsDeleted && x.FoodId == foodId && x.HouseholdId == householdId && x.IsPrefferedSupermarket);

        if (foodUoMId.HasValue)
        {
            var resultWithUoM = await query.Where(x => x.FoodUoMId == foodUoMId).FirstOrDefaultAsync();
            if (resultWithUoM != null)
                return resultWithUoM.SupermarketId;
        }

        var result = await query.FirstOrDefaultAsync();
        return result?.SupermarketId;
    }

    public async Task UpdateAsync(ShoppingListItem item)
    {
        dbContext.ShoppingListItems.Update(item);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(ShoppingListItem item)
    {
        dbContext.ShoppingListItems.Remove(item);
        await Task.CompletedTask;
    }
}