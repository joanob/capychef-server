using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodUoMDTO
{
    public FoodUoMDTO(FoodUoM foodUoM)
    {
        UoM = foodUoM.UoM;
        IsHouseholdUoM = foodUoM.HouseholdId.HasValue;
        IsBaseUoM = foodUoM.IsBaseUoM;
        Numerator = foodUoM.Numerator;
        Denominator = foodUoM.Denominator;
        IsApproxConversion = foodUoM.IsApproxConversion;
    }

    public string UoM { get; }
    public bool IsHouseholdUoM { get; }
    public bool IsBaseUoM { get; }
    public int? Numerator { get; }
    public int? Denominator { get; }
    public bool? IsApproxConversion { get; }
}