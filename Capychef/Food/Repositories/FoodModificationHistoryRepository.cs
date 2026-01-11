using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Repositories;

public class FoodModificationHistoryRepository(CapychefDbContext dbContext) : IFoodModificationHistoryRepository
{
    public async Task AddAsync(FoodModificationHistory foodModificationHistory)
    {
        await dbContext.FoodModificationsHistory.AddAsync(foodModificationHistory);
    }
}