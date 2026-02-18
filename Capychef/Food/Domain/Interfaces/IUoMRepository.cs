using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IUoMRepository
{
    Task AddAsync(UoM uom);
    Task<bool> CheckUoMExists(string uom);
    Task<List<UoM>> GetAllUoM();
}