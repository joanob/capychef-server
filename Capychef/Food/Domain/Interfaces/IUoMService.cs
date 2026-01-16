using Capychef.Food.Domain.Cmd;

namespace Capychef.Food.Domain.Interfaces;

public interface IUoMService
{
    Task LoadGlobalUoM(GlobalUoMFileCmd fileCmd);
}