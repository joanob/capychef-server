using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes_ingredients")]
public class RecipeIngredient
{
    protected RecipeIngredient()
    {
    }

    public RecipeIngredient(int recipeId, int orderNum, int foodId, double? quantity, int? foodUoMId, int createdBy)
    {
        RecipeId = recipeId;
        OrderNum = orderNum;
        FoodId = foodId;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    [Column("id")] public int Id { get; init; }

    [Column("recipe_id")] public int RecipeId { get; init; }

    [Column("order_num")] public int OrderNum { get; set; }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("quantity")] public double? Quantity { get; set; }

    [Column("food_uom_id")] public int? FoodUoMId { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }
}