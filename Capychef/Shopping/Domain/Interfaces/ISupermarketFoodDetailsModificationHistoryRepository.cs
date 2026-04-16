using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.Interfaces;

public interface ISupermarketFoodDetailsModificationHistoryRepository
{
    Task AddAsync(SupermarketFoodDetailsModificationHistory history);
}