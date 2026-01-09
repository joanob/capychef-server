using Capychef.Common.Auth;
using Capychef.Food.Domain.Cmd;
using YourOwnBoss.Common.Result;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task LoadGlobalFood(GlobalFoodFileCmd fileCmd);
    Task<List<FoodDTO>> GetAllGlobalFood();
    Task<Result<FoodDTO>> CreateHouseholdFood(AuthUserDetails userDetails, CreateHouseholdFoodCmd cmd);
}