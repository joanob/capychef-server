using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;

namespace Capychef.Food.Services;

public class FoodCategoryService(IFoodCategoryRepository foodCategoryRepository)
    : IFoodCategoryService
{
    public async Task<List<FoodCategoryDto>> GetAllCategories()
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var dtos = new List<FoodCategoryDto>();

        foreach (var category in categories) dtos.Add(new FoodCategoryDto(category));

        return dtos;
    }

    public async Task<List<FoodCategoryDto>> GetAllCategoriesAsTree()
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var dtos = new List<FoodCategoryDto>();

        foreach (var category in categories.Where(x => x.ParentCategoryId == null))
        {
            var dto = new FoodCategoryDto(category);
            dto.AddChildren(categories);
            dtos.Add(dto);
        }

        return dtos;
    }
}