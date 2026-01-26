using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodUoMRepository
{
    Task AddAsync(FoodUoM foodUoM);
    Task<List<FoodUoM>> GetTrackedAllUoMByFoodId(int foodId);
    Task<bool> CheckFoodUoMExistsById(int foodUoMId, int foodId);
}