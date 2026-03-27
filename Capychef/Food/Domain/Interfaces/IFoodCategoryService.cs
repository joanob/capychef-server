using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodCategoryService
{
    Task LoadFoodCategories(FoodCategoryFileCmd fileCmd);
    Task<List<FoodCategoryDto>> GetAllCategories();
    Task<List<FoodCategoryDto>> GetAllCategoriesAsTree();
}