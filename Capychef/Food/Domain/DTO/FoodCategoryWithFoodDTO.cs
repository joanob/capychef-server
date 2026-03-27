using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodCategoryWithFoodDto
{
    public FoodCategoryWithFoodDto(FoodCategory category)
    {
        Category = new FoodCategoryDto(category);
        Food = new List<FoodDto>();
        SubcategoriesWithFood = new List<FoodCategoryWithFoodDto>();
    }

    public FoodCategoryWithFoodDto(FoodCategory category, List<Entities.Food> food)
    {
        Category = new FoodCategoryDto(category);
        Food = FoodDto.ToList(food);
        SubcategoriesWithFood = new List<FoodCategoryWithFoodDto>();
    }

    public FoodCategoryDto Category { get; }
    public List<FoodDto> Food { get; }
    public List<FoodCategoryWithFoodDto> SubcategoriesWithFood { get; }

    public void AddFood(FoodDto food)
    {
        Food.Add(food);
    }

    private void AddSubcategoryWithFood(FoodCategoryWithFoodDto categoryWithFood)
    {
        SubcategoriesWithFood.Add(categoryWithFood);
    }

    public static List<FoodCategoryWithFoodDto> ToTree(List<FoodCategory> categories, List<Entities.Food> food)
    {
        var categoriesWithFoodDtoList = new List<FoodCategoryWithFoodDto>();

        foreach (var category in categories.Where(x => x.ParentCategoryId == null))
        {
            var categoryWithFoodDto =
                new FoodCategoryWithFoodDto(category, food.Where(x => x.CategoryId == category.Id).ToList());

            categoryWithFoodDto.AddSubcategoriesWithFood(categories, food);

            categoriesWithFoodDtoList.Add(categoryWithFoodDto);
        }

        return categoriesWithFoodDtoList;
    }

    private void AddSubcategoriesWithFood(List<FoodCategory> categories, List<Entities.Food> food)
    {
        foreach (var category in categories.Where(x => x.ParentCategoryId == Category.Id))
        {
            var subcategoryWithFoodDto =
                new FoodCategoryWithFoodDto(category, food.Where(x => x.CategoryId == category.Id).ToList());

            subcategoryWithFoodDto.AddSubcategoriesWithFood(categories, food);

            AddSubcategoryWithFood(subcategoryWithFoodDto);
        }
    }
}