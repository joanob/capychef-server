namespace Capychef.Data;

public class FoodDataFile
{
    public string GlobalId { get; set; }

    public string Name { get; set; }

    public int Category { get; set; }
    public int? DaysUntilExpiration { get; set; }
    public int? DaysUntilBestBefore { get; set; }

    public List<FoodUoMDataFile> UnitsOfMeasure { get; set; }
}