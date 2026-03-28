using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Food.Domain.Cmd;

public class FoodUoMCmd : ICmd
{
    public required string UoM { get; init; }
    public bool IsBaseUoM { get; init; }
    public int? Numerator { get; init; }
    public int? Denominator { get; init; }
    public bool? IsApproxConversion { get; init; }

    public ValidationError? Validate()
    {
        if (Numerator == null && Denominator == null && IsApproxConversion == null) return null;

        if (Numerator != null && Denominator != null && IsApproxConversion != null) return null;

        return new ValidationError("Invalid conversion details");
    }
}