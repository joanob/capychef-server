namespace Capychef.DataLoader.Entities;

public class FoodDataFile
{
    public required string GlobalId { get; init; }

    public required string Name { get; init; }

    public int Category { get; init; }
    public int? DaysUntilExpiration { get; init; }
    public int? DaysUntilBestBefore { get; init; }

    public List<FoodUoMDataFile> UnitsOfMeasure { get; init; } = [];
}