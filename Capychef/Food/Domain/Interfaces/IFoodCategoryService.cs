using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodCategoryService
{
    Task<List<FoodCategoryDto>> GetAllCategories();
    Task<List<FoodCategoryDto>> GetAllCategoriesAsTree();
}