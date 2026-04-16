using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;

namespace Capychef.Shopping.Services;

public class SupermarketFoodDetailsService(
    CapychefDbContext dbContext,
    ISupermarketFoodDetailsRepository repository,
    IFoodRepository foodRepository) : ISupermarketFoodDetailsService
{
    public async Task<Result<SupermarketFoodDetailsDto>> CreateSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, SupermarketFoodDetailsCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<SupermarketFoodDetailsDto>(validationError);

        var householdId = userDetails.GetHouseholdId();

        // Verify food exists and belongs to the household
        var food = await foodRepository.GetFoodById(foodId, householdId);
        if (food == null) return new Result<SupermarketFoodDetailsDto>(new NotFoundError(EntityType.Food, foodId));

        // Create new SupermarketFoodDetails entity
        var details = new SupermarketFoodDetails(
            householdId,
            foodId,
            cmd.SupermarketId,
            cmd.Price,
            cmd.Quantity,
            cmd.FoodUoMId,
            cmd.IsPrefferedSupermarket,
            userDetails.UserId);

        await repository.AddAsync(details);
        await dbContext.SaveChangesAsync();

        return new Result<SupermarketFoodDetailsDto>(new SupermarketFoodDetailsDto(details));
    }

    public async Task<AppError?> UpdateSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, int id, SupermarketFoodDetailsCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return validationError;

        var householdId = userDetails.GetHouseholdId();

        // Verify food exists and belongs to the household
        var food = await foodRepository.GetFoodById(foodId, householdId);
        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        // Find the existing SupermarketFoodDetails
        var details = await repository.FindTrackedById(id, householdId);
        if (details == null) return new NotFoundError(EntityType.SupermarketFoodDetails, id);

        // Verify the details belong to the requested food
        if (details.FoodId != foodId)
            return new NotFoundError(EntityType.SupermarketFoodDetails, id);

        // Update properties
        details.Price = cmd.Price;
        details.Quantity = cmd.Quantity;
        details.FoodUoMId = cmd.FoodUoMId;
        details.IsPrefferedSupermarket = cmd.IsPrefferedSupermarket;

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> DeleteSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, int id)
    {
        var householdId = userDetails.GetHouseholdId();

        // Verify food exists and belongs to the household
        var food = await foodRepository.GetFoodById(foodId, householdId);
        if (food == null) return new NotFoundError(EntityType.Food, foodId);

        // Find the existing SupermarketFoodDetails
        var details = await repository.FindTrackedById(id, householdId);
        if (details == null) return new NotFoundError(EntityType.SupermarketFoodDetails, id);

        // Verify the details belong to the requested food
        if (details.FoodId != foodId)
            return new NotFoundError(EntityType.SupermarketFoodDetails, id);

        // Delete (soft-delete)
        details.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }
}