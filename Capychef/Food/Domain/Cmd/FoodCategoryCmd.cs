namespace Capychef.Food.Domain.Cmd;

public class FoodCategoryCmd
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsLeaf { get; set; }

    public int? ParentCategoryId { get; set; }
}