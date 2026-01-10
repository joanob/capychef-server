namespace Capychef.Food.Domain.Interfaces;

public interface IFoodRepository
{
    Task AddAsync(Entities.Food food);
    Task<List<Entities.Food>> GetTrackedAllGlobalFood();
    Task<List<Entities.Food>> GetAllGlobalFood();
    Task<List<Entities.Food>> GetAllHouseholdFood(int householdId);
}