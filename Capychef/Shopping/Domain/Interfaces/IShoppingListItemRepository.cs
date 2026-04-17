using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.Interfaces;

public interface IShoppingListItemRepository
{
    Task AddAsync(ShoppingListItem item);

    Task<ShoppingListItem?> FindTrackedById(int id, int householdId);

    Task<SupermarketFoodDetails?> FindFirstSupermarketFoodDetailsByFoodId(int foodId, int householdId, int? foodUoMId);

    Task<int?> GetPreferredSupermarketId(int foodId, int? foodUoMId, int householdId);

    Task UpdateAsync(ShoppingListItem item);

    Task DeleteAsync(ShoppingListItem item);
}