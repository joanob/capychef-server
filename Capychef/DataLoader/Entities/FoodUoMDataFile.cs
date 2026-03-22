namespace Capychef.Data;

public class FoodUoMDataFile
{
    public string UoM { get; set; }
    public bool? IsBaseUoM { get; set; }
    public int? Numerator { get; set; }
    public int? Denominator { get; set; }
    public bool? IsApproxConversion { get; set; }
}