namespace Capychef.DataLoader.Entities;

public class FoodUoMDataFile
{
    public required string UoM { get; init; }
    public bool? IsBaseUoM { get; init; }
    public int? Numerator { get; init; }
    public int? Denominator { get; init; }
    public bool? IsApproxConversion { get; init; }
}