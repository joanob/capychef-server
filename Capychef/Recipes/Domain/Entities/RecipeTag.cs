using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes_tags")]
public class RecipeTag
{
    protected RecipeTag()
    {
        Tag = "";
    }

    public RecipeTag(int recipeId, int orderNum, string tag, int createdBy)
    {
        RecipeId = recipeId;
        OrderNum = orderNum;
        Tag = tag;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    [Column("id")] public int Id { get; init; }

    [Column("recipe_id")] public int RecipeId { get; init; }

    [Column("order_num")] public int OrderNum { get; set; }

    [Column("tag")] [MaxLength(50)] public string Tag { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }
}