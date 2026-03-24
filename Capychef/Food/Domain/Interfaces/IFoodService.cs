using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, HouseholdFoodCmd cmd);
    Task<List<FoodDTO>> GetAllHouseholdFood(AuthUserDetails userDetails);
    Task<List<FoodCategoryWithFoodDTO>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails);
    Task<AppError?> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails);
    Task<Result<FoodDTO>> UpdateFood(int id, HouseholdFoodCmd cmd, AuthUserDetails userDetails);
    Task<Result<FoodDTO>> GetFoodById(AuthUserDetails userDetails, int id);
}