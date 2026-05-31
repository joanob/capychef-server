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
    public async Task<List<FoodDto>> GetAllHouseholdFood(AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetAllHouseholdFood(userDetails.GetHouseholdId());

        var globalFood = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        food.AddRange(globalFood);

        return FoodDto.ToList(food);
    }

    public async Task<Result<FoodDto>> GetFoodById(AuthUserDetails userDetails, int id)
    {
        var food = await foodRepository.GetFoodById(id, userDetails.GetHouseholdId());

        if (food == null) return new Result<FoodDto>(new NotFoundError(EntityType.Food, id));

        return new Result<FoodDto>(new FoodDto(food));
    }

    public async Task<List<FoodCategoryWithFoodDto>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails)
    {
        var categories = await foodCategoryRepository.GetAllCategories();

        var food = await foodRepository.GetAllHouseholdFood(userDetails.GetHouseholdId());

        var globalFood = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        food.AddRange(globalFood);

        return FoodCategoryWithFoodDto.ToTree(categories, food);
    }

    public async Task<AppError?> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetTrackedHouseholdFoodById(foodId, userDetails.GetHouseholdId());

        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        food.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<FoodDto>> UpdateFood(int id, HouseholdFoodCmd cmd,
        AuthUserDetails userDetails)
    {
        var validationError = cmd.Validate();

        if (validationError != null) return new Result<FoodDto>(validationError);

        var food = await foodRepository.GetTrackedFoodById(id, userDetails.GetHouseholdId());

        if (food == null) return new Result<FoodDto>(new NotFoundError(EntityType.Food, id));

        // Some properties can only be updated on household food 

        if (food.HouseholdId.HasValue)
        {
            if (food.Name != cmd.Name)
            {
                await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                    userDetails.GetHouseholdId(),
                    FoodModifiableColumn.Name, food.Name,
                    cmd.Name, userDetails.UserId));

                food.Name = cmd.Name;
            }

            if (food.CategoryId != cmd.CategoryId)
            {
                var foodCategory = await foodCategoryRepository.GetTrackedCategoryById(cmd.CategoryId);
                if (foodCategory == null)
                    return new Result<FoodDto>(new NotFoundError(EntityType.FoodCategory, cmd.CategoryId));

                if (!foodCategory.IsLeaf)
                    return new Result<FoodDto>(new FoodCategoryCannotContainFood(foodCategory.Id));

                await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                    userDetails.GetHouseholdId(),
                    FoodModifiableColumn.CategoryId,
                    food.CategoryId.ToString(),
                    cmd.CategoryId.ToString(), userDetails.UserId));

                food.CategoryId = cmd.CategoryId;
            }
        }

        // Other properties are updated on HouseholdFoodDetails
        // If household food details have never been set, food.HouseholdFoodDetails will be null
        // Variable householdFoodDetails stores the initial state, wether is household food details or global details that pass to household food details
        // Each property is compared to this object to check if value was modified
        // Unmodified properties that have a global initial value (e.g, days until expiration) will be stored as HouseholdFoodDetails but not registered in FoodModificationHistory 

        var householdFoodDetails = food.HouseholdFoodDetails ?? new HouseholdFoodDetails(userDetails.GetHouseholdId(),
            food.Id, null, null, food.GetDaysUntilExpiration(), food.GetDaysUntilBestBefore());

        if ((householdFoodDetails.MinQuantity.HasValue && !cmd.MinQuantity.HasValue) ||
            (!householdFoodDetails.MinQuantity.HasValue && cmd.MinQuantity.HasValue) ||
            (householdFoodDetails.MinQuantity.HasValue && cmd.MinQuantity.HasValue &&
             Math.Abs(householdFoodDetails.MinQuantity.Value - cmd.MinQuantity.Value) < 1e-6))
        {
            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                userDetails.GetHouseholdId(),
                FoodModifiableColumn.MinQuantity, householdFoodDetails.MinQuantity?.ToString() ?? "",
                cmd.MinQuantity?.ToString() ?? "", userDetails.UserId));

            householdFoodDetails.MinQuantity = cmd.MinQuantity;

            food.SetHouseholdFoodDetails(householdFoodDetails);
        }

        if (householdFoodDetails.MinQuantityUoM != cmd.MinQuantityUoM)
        {
            // TODO: check food has UoM or UoM will be set on this update

            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                userDetails.GetHouseholdId(),
                FoodModifiableColumn.MinQuantityUoM, householdFoodDetails.MinQuantityUoM ?? "",
                cmd.MinQuantityUoM ?? "", userDetails.UserId));

            householdFoodDetails.MinQuantityUoM = cmd.MinQuantityUoM;

            food.SetHouseholdFoodDetails(householdFoodDetails);
        }

        if (householdFoodDetails.DaysUntilExpiration != cmd.DaysUntilExpiration)
        {
            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                userDetails.GetHouseholdId(),
                FoodModifiableColumn.DaysUntilExpiration,
                householdFoodDetails.DaysUntilExpiration?.ToString() ?? "",
                cmd.DaysUntilExpiration?.ToString() ?? "", userDetails.UserId));

            householdFoodDetails.DaysUntilExpiration = cmd.DaysUntilExpiration;

            food.SetHouseholdFoodDetails(householdFoodDetails);
        }

        if (householdFoodDetails.DaysUntilBestBefore != cmd.DaysUntilBestBefore)
        {
            await foodModificationHistoryRepository.AddAsync(new FoodModificationHistory(food.Id,
                userDetails.GetHouseholdId(),
                FoodModifiableColumn.DaysUntilBestBefore,
                householdFoodDetails.DaysUntilBestBefore?.ToString() ?? "",
                cmd.DaysUntilBestBefore?.ToString() ?? "", userDetails.UserId));

            householdFoodDetails.DaysUntilBestBefore = cmd.DaysUntilBestBefore;

            food.SetHouseholdFoodDetails(householdFoodDetails);
        }

        var error = await UpdateFoodUoM(food, cmd.UoM, userDetails);

        if (error != null)
            return new Result<FoodDto>(error);

        await dbContext.SaveChangesAsync();

        return new Result<FoodDto>(new FoodDto(food));
    }

    public async Task<Result<FoodDto>> CreateHouseholdFood(AuthUserDetails userDetails, HouseholdFoodCmd cmd)
    {
        var validationError = cmd.Validate();

        if (validationError != null) return new Result<FoodDto>(validationError);

        var foodCategory = await foodCategoryRepository.GetTrackedCategoryById(cmd.CategoryId);
        if (foodCategory == null)
            return new Result<FoodDto>(new NotFoundError(EntityType.FoodCategory, cmd.CategoryId));

        if (!foodCategory.IsLeaf)
            return new Result<FoodDto>(new FoodCategoryCannotContainFood(foodCategory.Id));

        var food = new Domain.Entities.Food(userDetails.GetHouseholdId(), cmd.Name, cmd.CategoryId,
            userDetails.UserId);

        food.AddHouseholdFoodDetails(userDetails.GetHouseholdId(), cmd.MinQuantity, cmd.MinQuantityUoM,
            cmd.DaysUntilExpiration, cmd.DaysUntilBestBefore);

        await foodRepository.AddAsync(food);

        foreach (var uom in cmd.UoM)
        {
            if (!await uoMRepository.CheckUoMExists(uom.UoM))
                return new Result<FoodDto>(new NotFoundError(EntityType.UoM, uom.UoM));

            await foodUoMRepository.AddAsync(new FoodUoM(food, userDetails.GetHouseholdId(), uom.UoM, uom.IsBaseUoM,
                null, null, null));
        }

        await dbContext.SaveChangesAsync();

        return new Result<FoodDto>(new FoodDto(food));
    }

    /**
     * UpdateFoodUoM sets the list of food units of measure for user's household
     *
     * The list will contain the complete list of food units of measure for the food, not just the ones to be updated. Those units of measure that are in the database but not in the list will be deleted as household food units of measure
     */
    private async Task<AppError?> UpdateFoodUoM(Domain.Entities.Food food, List<FoodUoMCmd> cmd,
        AuthUserDetails userDetails)
    {
        if (cmd.Count(x => x.IsBaseUoM) != 1) return new FoodBaseUoMNotFound(food.Id);

        var foodUoMs = food.UoM.ToList();

        foreach (var uomCmd in cmd)
        {
            var validationError = uomCmd.Validate();

            if (validationError != null) return new FoodUoMConversionError(food.Id, uomCmd.UoM);

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

        return null;
    }

    public async Task<List<FoodDto>> GetAllGlobalFood(AuthUserDetails userDetails)
    {
        var food = await foodRepository.GetAllGlobalFood(userDetails.GetHouseholdId());

        return FoodDto.ToList(food);
    }
}