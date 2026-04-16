using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.Interfaces;

public interface ISupermarketFoodDetailsRepository
{
    Task AddAsync(SupermarketFoodDetails details);

    Task<SupermarketFoodDetails?> FindTrackedById(int id, int householdId);

    Task<SupermarketFoodDetails?> FindTrackedByFoodAndSupermarket(int foodId, int supermarketId, int householdId);

    Task DeleteAsync(SupermarketFoodDetails details);
}