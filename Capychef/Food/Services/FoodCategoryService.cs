using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Services;

public class FoodCategoryService(CapychefDbContext dbContext, IFoodCategoryRepository foodCategoryRepository)
    : IFoodCategoryService
{
    public async Task LoadFoodCategories(FoodCategoryFileCmd fileCmd)
    {
        var categories = await foodCategoryRepository.GetTrackedAllCategories();

        foreach (var categoryCmd in fileCmd.FoodCategories)
        {
            var storedCategory = categories.FirstOrDefault(x => x.Id == categoryCmd.Id);
            if (storedCategory != null)
            {
                storedCategory.Name = categoryCmd.Name;
                storedCategory.IsLeaf = categoryCmd.IsLeaf;
                storedCategory.ParentCategoryId = categoryCmd.ParentCategoryId;
            }
            else
            {
                var category = new FoodCategory(categoryCmd.Id, categoryCmd.Name, categoryCmd.IsLeaf,
                    categoryCmd.ParentCategoryId);

                await dbContext.FoodCategories.AddAsync(category);
            }
        }

        foreach (var category in categories)
            if (!fileCmd.FoodCategories.Any(x => x.Id == category.Id))
                dbContext.Remove(category);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<FoodCategoryDTO>> GetAllCategories()
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var dtos = new List<FoodCategoryDTO>();

        foreach (var category in categories) dtos.Add(new FoodCategoryDTO(category));

        return dtos;
    }

    public async Task<List<FoodCategoryDTO>> GetAllCategoriesAsTree()
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var dtos = new List<FoodCategoryDTO>();

        foreach (var category in categories.Where(x => x.ParentCategoryId == null))
        {
            var dto = new FoodCategoryDTO(category);
            dto.AddChildren(categories);
            dtos.Add(dto);
        }

        return dtos;
    }
}