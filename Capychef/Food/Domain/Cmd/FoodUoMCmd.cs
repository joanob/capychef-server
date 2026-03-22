namespace Capychef.Food.Domain.Cmd;

public class FoodUoMCmd
{
    public string UoM { get; set; }
    public bool IsBaseUoM { get; set; }
    public int? Numerator { get; }
    public int? Denominator { get; }
    public bool? IsApproxConversion { get; }

    public bool CheckConversion()
    {
        if (Numerator == null && Denominator == null && IsApproxConversion == null) return true;

        if (Numerator != null && Denominator != null && IsApproxConversion != null) return true;

        return false;
    }
}