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
        var food = await foodRepository.GetAllHouseholdFood(userDetails.GetHouseholdId());

        var globalFood = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        food.AddRange(globalFood);

        return FoodDTO.ToList(food);
    }

    public async Task<Result<FoodDTO>> GetFoodById(AuthUserDetails userDetails, int id)
    {
        var food = await foodRepository.GetFoodById(id, userDetails.GetHouseholdId());

        if (food == null) return new Result<FoodDTO>(new NotFoundError(EntityType.Food, id));

        return new Result<FoodDTO>(new FoodDTO(food));
    }

    /**
     * UpdateFoodUoM sets the list of food units of measure for user's household
     * 
     * The list will contain the complete list of food units of measure for the food, not just the ones to be updated. Those units of measure that are in the database but not in the list will be deleted as household food units of measure
     */
    public async Task<AppError> UpdateFoodUoM(int foodId, List<FoodUoMCmd> cmd, AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedGlobalFoodById(foodId, userDetails.GetHouseholdId());
        if (food == null) food = await foodRepository.GetTrackedFoodById(foodId, userDetails.GetHouseholdId());

        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        if (cmd.Count(x => x.IsBaseUoM) != 1) return new FoodBaseUoMNotFound(foodId);

        var foodUoMs = food.UoM.ToList();

        foreach (var uomCmd in cmd)
        {
            if (!uomCmd.CheckConversion()) return new FoodUoMConversionError(foodId, uomCmd.UoM);

            var foodUoM =
                foodUoMs.FirstOrDefault(x => x.UoM == uomCmd.UoM && x.HouseholdId == userDetails.GetHouseholdId());
            if (foodUoM == null) foodUoM = foodUoMs.FirstOrDefault(x => x.UoM == uomCmd.UoM && x.HouseholdId == null);

            if (foodUoM == null)
            {
                await foodUoMRepository.AddAsync(new FoodUoM(food.Id, userDetails.GetHouseholdId(), uomCmd.UoM,
                    uomCmd.IsBaseUoM, uomCmd.Numerator, uomCmd.Denominator, uomCmd.IsApproxConversion));
            }
            else
            {
                if (foodUoM.IsDeleted)
                {
                    if (!foodUoM.HouseholdId.HasValue)
                        await foodUoMRepository.AddAsync(new FoodUoM(food.Id, userDetails.GetHouseholdId(), uomCmd.UoM,
                            uomCmd.IsBaseUoM, uomCmd.Numerator, uomCmd.Denominator, uomCmd.IsApproxConversion));
                    else
                        foodUoM.RestoreDeleted();
                }

                // If existing food uom belongs to household, update it. If it belongs to global food, only add a new household food uom if the conversion details are different, otherwise keep using the global food uom
                if (foodUoM.HouseholdId.HasValue)
                {
                    foodUoM.Set(uomCmd.IsBaseUoM, uomCmd.Numerator, uomCmd.Denominator, uomCmd.IsApproxConversion);
                }
                else
                {
                    if (foodUoM.IsBaseUoM != uomCmd.IsBaseUoM || foodUoM.Numerator != uomCmd.Numerator ||
                        foodUoM.Denominator != uomCmd.Denominator ||
                        foodUoM.IsApproxConversion != uomCmd.IsApproxConversion)
                        await foodUoMRepository.AddAsync(new FoodUoM(food.Id, userDetails.GetHouseholdId(), uomCmd.UoM,
                            uomCmd.IsBaseUoM, uomCmd.Numerator, uomCmd.Denominator, uomCmd.IsApproxConversion));
                }
            }
        }

        foreach (var foodUoM in foodUoMs)
            if (cmd.All(x => x.UoM != foodUoM.UoM))
            {
                if (foodUoM.HouseholdId.HasValue)
                {
                    foodUoM.Delete();
                }
                else
                {
                    var deletedFoodUoM = new FoodUoM(food.Id, userDetails.GetHouseholdId(), foodUoM.UoM,
                        foodUoM.IsBaseUoM, foodUoM.Numerator, foodUoM.Denominator, foodUoM.IsApproxConversion);

                    deletedFoodUoM.Delete();

                    await foodUoMRepository.AddAsync(deletedFoodUoM);
                }
            }

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<List<FoodCategoryWithFoodDTO>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails)
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var food = await foodRepository.GetAllHouseholdFood(userDetails.GetHouseholdId());

        var globalFood = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        food.AddRange(globalFood);

        return FoodCategoryWithFoodDTO.ToTree(categories, food);
    }

    public async Task<AppError?> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(foodId, userDetails.GetHouseholdId());

        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        food.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<FoodDTO>> UpdateHouseholdFood(int id, UpdateHouseholdFoodCmd cmd,
        AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(id, userDetails.GetHouseholdId());

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

        foreach (var uom in cmd.UoM)
        {
            if (!await uoMRepository.CheckUoMExists(uom.UoM))
                return new Result<FoodDTO>(new NotFoundError(EntityType.UoM, uom.UoM));

            if (food.UoM.All(x => x.UoM != uom.UoM))
            {
                food.AddUoM(new FoodUoM(food, userDetails.GetHouseholdId(), uom.UoM, false, null, null, null));

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

        var food = new Domain.Entities.Food(userDetails.GetHouseholdId(), cmd.Name, cmd.CategoryId,
            userDetails.UserId, cmd.DaysUntilExpiration, cmd.DaysUntilBestBefore);

        await foodRepository.AddAsync(food);

        if (cmd.UoM != null)
            foreach (var uom in cmd.UoM)
            {
                if (!await uoMRepository.CheckUoMExists(uom.UoM))
                    return new Result<FoodDTO>(new NotFoundError(EntityType.UoM, uom.UoM));

                await foodUoMRepository.AddAsync(new FoodUoM(food, userDetails.GetHouseholdId(), uom.UoM, uom.IsBaseUoM,
                    null, null, null));
            }

        await dbContext.SaveChangesAsync();

        return new Result<FoodDTO>(new FoodDTO(food));
    }

    public async Task<List<FoodDTO>> GetAllGlobalFood(AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        return FoodDTO.ToList(food);
    }
}