namespace Capychef.Food.Domain.Cmd;

public class FoodUoMCmd
{
    public required string UoM { get; init; }
    public bool IsBaseUoM { get; init; }
    public int? Numerator { get; init; }
    public int? Denominator { get; init; }
    public bool? IsApproxConversion { get; init; }

    public bool CheckConversion()
    {
        if (Numerator == null && Denominator == null && IsApproxConversion == null) return true;

        if (Numerator != null && Denominator != null && IsApproxConversion != null) return true;

        return false;
    }
}