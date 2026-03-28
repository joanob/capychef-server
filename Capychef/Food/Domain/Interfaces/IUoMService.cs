using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IUoMService
{
    Task<List<UoMdto>> GetAllUoM();
}