namespace Capychef.Food.Domain.Entities;

/// <summary>
///     Represents the result of a unit-of-measure conversion, including whether the conversion was approximate
///     (e.g. converting between weight and count for fruit/vegetables).
/// </summary>
public record ConversionResult(double Quantity, bool IsApproximate);