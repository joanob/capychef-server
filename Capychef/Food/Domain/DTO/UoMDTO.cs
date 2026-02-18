using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class UoMDTO
{
    public UoMDTO(UoM uom)
    {
        Code = uom.Code;
        Name = uom.Name;
        DimensionCode = uom.DimensionCode;
        BaseUoM = uom.BaseUoM;
        Numerator = uom.Numerator;
        Denominator = uom.Denominator;
    }

    public string Code { get; }

    public string Name { get; }

    public string DimensionCode { get; }

    public string? BaseUoM { get; }

    public int? Numerator { get; }

    public int? Denominator { get; }

    public static List<UoMDTO> ToList(List<UoM> uoms)
    {
        return uoms.Select(x => new UoMDTO(x)).ToList();
    }
}