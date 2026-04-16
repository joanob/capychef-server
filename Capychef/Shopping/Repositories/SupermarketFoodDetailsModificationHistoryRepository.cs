using Capychef.Persistence;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;

namespace Capychef.Shopping.Repositories;

public class SupermarketFoodDetailsModificationHistoryRepository(CapychefDbContext dbContext)
    : ISupermarketFoodDetailsModificationHistoryRepository
{
    public async Task AddAsync(SupermarketFoodDetailsModificationHistory history)
    {
        await dbContext.SupermarketFoodDetailsModificationsHistory.AddAsync(history);
    }
}