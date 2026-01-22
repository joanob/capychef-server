using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodUoMDTO
{
    public FoodUoMDTO(FoodUoM foodUoM)
    {
        Code = foodUoM.UoM;
        BaseUoM = foodUoM.BaseUoM;
        Numerator = foodUoM.Numerator;
        Denominator = foodUoM.Denominator;
    }

    public string Code { get; }
    public string? BaseUoM { get; }
    public int? Numerator { get; }
    public int? Denominator { get; }
}