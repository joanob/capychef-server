using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodCategoryRepository
{
    Task<List<FoodCategory>> GetTrackedAllCategories();
    Task<List<FoodCategory>> GetAllCategories();
    Task<bool> CheckCategoryExistsById(int id);
}