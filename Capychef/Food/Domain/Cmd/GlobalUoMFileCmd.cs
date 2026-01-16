using System.Text.Json.Serialization;

namespace Capychef.Food.Domain.Cmd;

public class GlobalUoMFileCmd
{
    [JsonPropertyName("uomDimensions")] public List<GlobalUoMDimensionCmd> UomDimensions { get; set; }

    public List<GlobalUoMCmd> Uom { get; set; }
}

public class GlobalUoMDimensionCmd
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class GlobalUoMCmd
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string DimensionCode { get; set; }
    public string? BaseUom { get; set; }
    public int? Numerator { get; set; }
    public int? Denominator { get; set; }
}