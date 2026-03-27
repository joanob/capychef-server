namespace Capychef.DataLoader.Entities;

public class UoMDataFile
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string DimensionCode { get; init; }
    public string? BaseUom { get; init; }
    public int? Numerator { get; init; }
    public int? Denominator { get; init; }
}