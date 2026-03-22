namespace Capychef.Food.Domain.Interfaces;

public interface IFoodRepository
{
    Task AddAsync(Entities.Food food);
    Task<List<Entities.Food>> GetTrackedAllGlobalFood(int? householdId);
    Task<List<Entities.Food>> GetAllGlobalFood(int? householdId);
    Task<List<Entities.Food>> GetAllHouseholdFood(int householdId);
    Task<Entities.Food?> GetGlobalFoodById(int foodId, int? householdId);
    Task<Entities.Food?> GetTrackedGlobalFoodById(int foodId, int? householdId);
    Task<Entities.Food?> GetTrackedHouseholdFoodById(int foodId, int householdId);
    Task<bool> CheckFoodExistsById(int cmdFoodId, int householdId);
    Task<Entities.Food?> GetFoodById(int id, int householdId);
    Task<Entities.Food?> GetTrackedFoodById(int id, int householdId);
}