using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Services;

public class FoodService(CapychefDbContext dbContext, IFoodRepository foodRepository)
    : IFoodService
{
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