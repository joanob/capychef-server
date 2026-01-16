namespace Capychef.Food.Domain.Cmd;

public class UpdateHouseholdFoodCmd
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string BaseUoM { get; set; }
}