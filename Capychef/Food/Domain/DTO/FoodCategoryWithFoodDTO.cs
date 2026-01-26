using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodCategoryWithFoodDTO
{
    public FoodCategoryWithFoodDTO(FoodCategory category)
    {
        Category = new FoodCategoryDTO(category);
        Food = new List<FoodDTO>();
        SubcategoriesWithFood = new List<FoodCategoryWithFoodDTO>();
    }

    public FoodCategoryWithFoodDTO(FoodCategory category, List<Entities.Food> food)
    {
        Category = new FoodCategoryDTO(category);
        Food = FoodDTO.ToList(food);
        SubcategoriesWithFood = new List<FoodCategoryWithFoodDTO>();
    }

    public FoodCategoryDTO Category { get; }
    public List<FoodDTO> Food { get; }
    public List<FoodCategoryWithFoodDTO> SubcategoriesWithFood { get; }

    public void AddFood(FoodDTO food)
    {
        Food.Add(food);
    }

    private void AddSubcategoryWithFood(FoodCategoryWithFoodDTO categoryWithFood)
    {
        SubcategoriesWithFood.Add(categoryWithFood);
    }

    public static List<FoodCategoryWithFoodDTO> ToTree(List<FoodCategory> categories, List<Entities.Food> food)
    {
        var categoriesWithFoodDTOList = new List<FoodCategoryWithFoodDTO>();

        foreach (var category in categories.Where(x => x.ParentCategoryId == null))
        {
            var categoryWithFoodDTO =
                new FoodCategoryWithFoodDTO(category, food.Where(x => x.CategoryId == category.Id).ToList());

            categoryWithFoodDTO.AddSubcategoriesWithFood(categories, food);

            categoriesWithFoodDTOList.Add(categoryWithFoodDTO);
        }

        return categoriesWithFoodDTOList;
    }

    private void AddSubcategoriesWithFood(List<FoodCategory> categories, List<Entities.Food> food)
    {
        foreach (var category in categories.Where(x => x.ParentCategoryId == Category.Id))
        {
            var subcategoryWithFoodDto =
                new FoodCategoryWithFoodDTO(category, food.Where(x => x.CategoryId == category.Id).ToList());

            subcategoryWithFoodDto.AddSubcategoriesWithFood(categories, food);

            AddSubcategoryWithFood(subcategoryWithFoodDto);
        }
    }
}