using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Errors;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Services;

public class FoodService(
    CapychefDbContext dbContext,
    IFoodRepository foodRepository,
    IFoodCategoryRepository foodCategoryRepository,
    IFoodUoMRepository foodUoMRepository,
    IUoMRepository uoMRepository,
    IFoodModificationHistoryRepository foodModificationHistoryRepository)
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

    public async Task<Result<FoodDTO>> UpdateHouseholdFood(int id, UpdateHouseholdFoodCmd cmd,
        AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(id, userDetails.HouseholdId.Value);

        if (food == null) return new Result<FoodDTO>(new NotFoundError(EntityType.Food, id));

        if (food.Name != cmd.Name)
        {
            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                FoodModifiableColumn.Name, food.Name,
                cmd.Name, userDetails.UserId));

            food.Name = cmd.Name;
        }

        if (food.CategoryId != cmd.CategoryId)
        {
            var foodCategory = await foodCategoryRepository.GetTrackedCategoryById(cmd.CategoryId);
            if (foodCategory == null)
                return new Result<FoodDTO>(new NotFoundError(EntityType.FoodCategory, cmd.CategoryId));

            if (!foodCategory.IsLeaf)
                return new Result<FoodDTO>(new FoodCategoryCannotContainFood(foodCategory.Id));

            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                FoodModifiableColumn.CategoryId,
                food.CategoryId.ToString(),
                cmd.CategoryId.ToString(), userDetails.UserId));

            food.CategoryId = cmd.CategoryId;
        }

        if (food.BaseUoM != cmd.BaseUoM)
        {
            if (!await uoMRepository.CheckUoMExists(cmd.BaseUoM))
                return new Result<FoodDTO>(new NotFoundError(EntityType.UoM, cmd.BaseUoM));

            if (food.UoM.All(x => x.UoM != cmd.BaseUoM) && cmd.UoM.All(x => x.UoM != cmd.BaseUoM))
                return new Result<FoodDTO>(new FoodBaseUoMNotFound(food.Id, cmd.BaseUoM));

            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                FoodModifiableColumn.BaseUoM,
                food.BaseUoM,
                cmd.BaseUoM, userDetails.UserId));

            food.BaseUoM = cmd.BaseUoM;
        }

        foreach (var uom in cmd.UoM)
        {
            if (!await uoMRepository.CheckUoMExists(uom.UoM))
                return new Result<FoodDTO>(new NotFoundError(EntityType.UoM, uom.UoM));

            if (food.UoM.All(x => x.UoM != uom.UoM))
            {
                food.AddUoM(uom.UoM);

                await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                    FoodModifiableColumn.UoM,
                    "",
                    uom.UoM, userDetails.UserId));
            }
        }

        var deletedUoM = new List<string>();

        foreach (var uom in food.UoM)
            if (cmd.UoM.All(x => x.UoM != uom.UoM))
            {
                deletedUoM.Add(uom.UoM);

                await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                    FoodModifiableColumn.UoM, uom.UoM,
                    "",
                    userDetails.UserId));
            }

        foreach (var uom in deletedUoM) food.DeleteUom(uom);

        await dbContext.SaveChangesAsync();

        return new Result<FoodDTO>(new FoodDTO(food));
    }

    public async Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, CreateHouseholdFoodCmd cmd)
    {
        var foodCategory = await foodCategoryRepository.GetTrackedCategoryById(cmd.CategoryId);
        if (foodCategory == null)
            return new Result<FoodDTO>(new NotFoundError(EntityType.FoodCategory, cmd.CategoryId));

        if (!foodCategory.IsLeaf)
            return new Result<FoodDTO>(new FoodCategoryCannotContainFood(foodCategory.Id));

        if (cmd.UoM.All(x => x.UoM != cmd.BaseUoM))
            return new Result<FoodDTO>(new FoodBaseUoMNotFound(0, cmd.BaseUoM));

        var food = new Domain.Entities.Food(userDetails.HouseholdId.Value, cmd.Name, cmd.CategoryId, cmd.BaseUoM,
            userDetails.UserId, cmd.DaysUntilExpiration, cmd.DaysUntilBestBefore);

        await foodRepository.AddAsync(food);

        if (cmd.UoM != null)
            foreach (var uom in cmd.UoM)
            {
                if (!await uoMRepository.CheckUoMExists(uom.UoM))
                    return new Result<FoodDTO>(new NotFoundError(EntityType.UoM, uom.UoM));

                await foodUoMRepository.AddAsync(new FoodUoM(food, uom.UoM, null, null, null));
            }

        await dbContext.SaveChangesAsync();

        return new Result<FoodDTO>(new FoodDTO(food));
    }

    /**
     * Create or update global food
     * 
     * For compatibility reasons, global food cannot be deleted
     */
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
                modifiedGlobalFood.BaseUoM = food.BaseUoM;

                var globalFoodUoM = await foodUoMRepository.GetTrackedAllUoMByFoodId(modifiedGlobalFood.Id);

                foreach (var uom in food.UnitsOfMeasure)
                {
                    var modifiedGlobalFoodUoM = globalFoodUoM.FirstOrDefault(x => x.UoM == uom.UoM);
                    if (modifiedGlobalFoodUoM == null)
                        await foodUoMRepository.AddAsync(new FoodUoM(modifiedGlobalFood.Id, uom.UoM, null, null, null));
                }
            }
            else
            {
                var newGlobalFood = new Domain.Entities.Food(food.GlobalId, food.Name, food.Category, food.BaseUoM,
                    food.DaysUntilExpiration, food.DaysUntilBestBefore);

                await foodRepository.AddAsync(newGlobalFood);

                foreach (var uom in food.UnitsOfMeasure)
                    await foodUoMRepository.AddAsync(new FoodUoM(newGlobalFood, uom.UoM, null, null, null));
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<FoodDTO>> GetAllGlobalFood()
    {
        var food = await foodRepository.GetAllGlobalFood();

        return FoodDTO.ToList(food);
    }
}