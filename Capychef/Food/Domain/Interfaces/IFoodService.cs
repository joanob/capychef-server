using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task LoadGlobalFood(GlobalFoodFileCmd fileCmd);
    Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, CreateHouseholdFoodCmd cmd);
    Task<List<FoodDTO>> GetAllHouseholdFood(AuthUserDetails userDetails);
    Task<List<FoodCategoryWithFoodDTO>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails);
    Task<AppError> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails);
    Task<Result<FoodDTO>> UpdateHouseholdFood(int id, UpdateHouseholdFoodCmd cmd, AuthUserDetails userDetails);
    Task<Result<FoodDTO>> GetFoodById(AuthUserDetails userDetails, int id);
}