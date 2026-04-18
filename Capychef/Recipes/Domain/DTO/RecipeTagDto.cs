using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.DTO;

public class RecipeTagDto
{
    public RecipeTagDto(RecipeTag tag)
    {
        Id = tag.Id;
        RecipeId = tag.RecipeId;
        OrderNum = tag.OrderNum;
        Tag = tag.Tag;
        CreatedAt = tag.CreatedAt;
        CreatedBy = tag.CreatedBy;
    }

    public int Id { get; init; }
    public int RecipeId { get; init; }
    public int OrderNum { get; init; }
    public string Tag { get; init; }
    public DateTime CreatedAt { get; init; }
    public int CreatedBy { get; init; }
}