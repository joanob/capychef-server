namespace Capychef.Data;

public class UoMDataFile
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string DimensionCode { get; set; }
    public string? BaseUom { get; set; }
    public int? Numerator { get; set; }
    public int? Denominator { get; set; }
}