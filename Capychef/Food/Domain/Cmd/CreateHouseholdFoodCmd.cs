namespace Capychef.Food.Domain.Cmd;

public class CreateHouseholdFoodCmd
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public int? DaysUntilExpiration { get; set; }
    public int? DaysUntilBestBefore { get; set; }
    public List<FoodUoMCmd> UoM { get; set; }
}