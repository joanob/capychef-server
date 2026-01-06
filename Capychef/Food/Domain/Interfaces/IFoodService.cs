using Capychef.Food.Domain.Cmd;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodService
{
    Task LoadGlobalFood(GlobalFoodFileCmd fileCmd);
    Task<List<FoodDTO>> GetAllGlobalFood();
}