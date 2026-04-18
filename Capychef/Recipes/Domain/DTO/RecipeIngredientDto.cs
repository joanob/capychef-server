using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.DTO;

public class RecipeIngredientDto
{
    public RecipeIngredientDto(RecipeIngredient ingredient)
    {
        Id = ingredient.Id;
        RecipeId = ingredient.RecipeId;
        OrderNum = ingredient.OrderNum;
        FoodId = ingredient.FoodId;
        Quantity = ingredient.Quantity;
        FoodUoMId = ingredient.FoodUoMId;
        AlternativeTo = ingredient.AlternativeTo;
        CreatedAt = ingredient.CreatedAt;
        CreatedBy = ingredient.CreatedBy;
    }

    public int Id { get; init; }
    public int RecipeId { get; init; }
    public int OrderNum { get; init; }
    public int FoodId { get; init; }
    public double? Quantity { get; init; }
    public int? FoodUoMId { get; init; }
    public int? AlternativeTo { get; init; }
    public DateTime CreatedAt { get; init; }
    public int CreatedBy { get; init; }
}