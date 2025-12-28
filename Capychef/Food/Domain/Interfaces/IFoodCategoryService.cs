using Capychef.Food.Domain.Cmd;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodCategoryService
{
    Task LoadFoodCategories(FoodCategoryFileCmd fileCmd);
    Task<List<FoodCategoryDTO>> GetAllCategories();
    Task<List<FoodCategoryDTO>> GetAllCategoriesAsTree();
}