using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task<Result<FoodDto>> CreateHouseholdFood(AuthUserDetails userDetails, HouseholdFoodCmd cmd);
    Task<List<FoodDto>> GetAllHouseholdFood(AuthUserDetails userDetails);
    Task<List<FoodCategoryWithFoodDto>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails);
    Task<AppError?> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails);
    Task<Result<FoodDto>> UpdateFood(int id, HouseholdFoodCmd cmd, AuthUserDetails userDetails);
    Task<Result<FoodDto>> GetFoodById(AuthUserDetails userDetails, int id);
}