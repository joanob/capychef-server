using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;

namespace Capychef.Food.Services;

public class UoMService(
    IUoMRepository uoMRepository)
    : IUoMService
{
    public async Task<List<UoMdto>> GetAllUoM()
    {
        var uoMs = await uoMRepository.GetAllUoM();

        return UoMdto.ToList(uoMs);
    }
}