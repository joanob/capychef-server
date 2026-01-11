using Capychef.Common.Auth;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Food.Services;

public class FoodService(
    CapychefDbContext dbContext,
    IFoodRepository foodRepository,
    IFoodCategoryRepository foodCategoryRepository)
    : IFoodService
{
    public async Task<List<FoodDTO>> GetAllHouseholdFood(AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetAllHouseholdFood(userDetails.HouseholdId.Value);

        var globalFood = await foodRepository.GetAllGlobalFood();

        food.AddRange(globalFood);

        return FoodDTO.ToList(food);
    }

    public async Task<List<FoodCategoryWithFoodDTO>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails)
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var food = await foodRepository.GetAllHouseholdFood(userDetails.HouseholdId.Value);

        var globalFood = await foodRepository.GetAllGlobalFood();

        food.AddRange(globalFood);

        return FoodCategoryWithFoodDTO.ToTree(categories, food);
    }

    public async Task<AppError> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(foodId, userDetails.HouseholdId.Value);

        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        food.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, CreateHouseholdFoodCmd cmd)
    {
        if (!await foodCategoryRepository.CheckCategoryExistsById(cmd.CategoryId))
            return new Result<FoodDTO>(new NotFoundError(EntityType.FoodCategory, cmd.CategoryId));

        var food = new Domain.Entities.Food(userDetails.HouseholdId.Value, cmd.Name, cmd.CategoryId,
            userDetails.UserId);

        await foodRepository.AddAsync(food);

        await dbContext.SaveChangesAsync();

        return new Result<FoodDTO>(new FoodDTO(food));
    }

    public async Task LoadGlobalFood(GlobalFoodFileCmd fileCmd)
    {
        var globalFood = await foodRepository.GetTrackedAllGlobalFood();

        foreach (var food in fileCmd.Food)
        {
            var modifiedGlobalFood = globalFood.FirstOrDefault(x => x.GlobalId == food.GlobalId);
            if (modifiedGlobalFood != null)
            {
                modifiedGlobalFood.Name = food.Name;
                modifiedGlobalFood.CategoryId = food.Category;
            }
            else
            {
                var newGlobalFood = new Domain.Entities.Food(food.GlobalId, food.Name, food.Category);

                foodRepository.AddAsync(newGlobalFood);
            }
        }

        // TODO: handle deletion

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<FoodDTO>> GetAllGlobalFood()
    {
        var food = await foodRepository.GetAllGlobalFood();

        return FoodDTO.ToList(food);
    }
}