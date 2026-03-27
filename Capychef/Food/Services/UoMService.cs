using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Services;

public class UoMService(
    CapychefDbContext dbContext,
    IUoMDimensionRepository uoMDimensionRepository,
    IUoMRepository uoMRepository)
    : IUoMService
{
    public async Task LoadGlobalUoM(GlobalUoMFileCmd fileCmd)
    {
        foreach (var uomDimension in fileCmd.UomDimensions)
            await uoMDimensionRepository.AddAsync(new UoMDimension(uomDimension.Code, uomDimension.Name));

        foreach (var uom in fileCmd.Uom)
            await uoMRepository.AddAsync(new UoM(uom.Code, uom.Name, uom.DimensionCode, uom.BaseUom, uom.Numerator,
                uom.Denominator));

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<UoMdto>> GetAllUoM()
    {
        var uoMs = await uoMRepository.GetAllUoM();

        return UoMdto.ToList(uoMs);
    }
}