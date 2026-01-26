namespace Capychef.Food.Domain.Cmd;

public class GlobalFoodCmd
{
    public string GlobalId { get; set; }

    public string Name { get; set; }

    public int Category { get; set; }
    public string BaseUoM { get; set; }
    public int? DaysUntilExpiration { get; set; }
    public int? DaysUntilBestBefore { get; set; }

    public List<FoodUoMCmd> UnitsOfMeasure { get; set; }
}