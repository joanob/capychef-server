using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IFoodModificationHistoryRepository
{
    Task AddAsync(FoodModificationHistory foodModificationHistory);
}