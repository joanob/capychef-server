using Capychef.Common.Auth;
using Capychef.Food.Domain.Cmd;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task LoadGlobalFood(GlobalFoodFileCmd fileCmd);
    Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, CreateHouseholdFoodCmd cmd);
    Task<List<FoodDTO>> GetAllHouseholdFood(AuthUserDetails userDetails);
    Task<List<FoodCategoryWithFoodDTO>> GetAllHouseholdFoodGroupedByCategory(AuthUserDetails userDetails);
    Task<AppError> DeleteHouseholdFood(int foodId, AuthUserDetails userDetails);
}