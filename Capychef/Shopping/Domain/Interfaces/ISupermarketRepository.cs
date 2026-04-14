using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.Interfaces;

public interface ISupermarketRepository
{
    Task AddAsync(Supermarket supermarket);
    Task<Supermarket?> FindTrackedById(int id);
}