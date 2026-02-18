using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;

namespace Capychef.Food.Domain.Interfaces;

public interface IUoMService
{
    Task LoadGlobalUoM(GlobalUoMFileCmd fileCmd);
    Task<List<UoMDTO>> GetAllUoM();
}