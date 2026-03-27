namespace Capychef.Food.Domain.Cmd;

public class FoodCategoryCmd
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public bool IsLeaf { get; init; }

    public int? ParentCategoryId { get; init; }
}