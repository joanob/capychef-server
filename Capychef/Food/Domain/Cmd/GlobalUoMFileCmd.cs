using System.Text.Json.Serialization;

namespace Capychef.Food.Domain.Cmd;

public class GlobalUoMFileCmd
{
    [JsonPropertyName("uomDimensions")] public required List<GlobalUoMDimensionCmd> UomDimensions { get; init; } = [];

    public required List<GlobalUoMCmd> Uom { get; init; } = [];
}

public class GlobalUoMDimensionCmd
{
    public required string Code { get; init; }
    public required string Name { get; init; }
}

public class GlobalUoMCmd
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string DimensionCode { get; init; }
    public string? BaseUom { get; init; }
    public int? Numerator { get; init; }
    public int? Denominator { get; init; }
}