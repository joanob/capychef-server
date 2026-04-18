using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes_steps")]
public class RecipeStep
{
    protected RecipeStep()
    {
    }

    public RecipeStep(int recipeId, int stepNumber, string description, int createdBy)
    {
        RecipeId = recipeId;
        StepNumber = stepNumber;
        Description = description;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    [Column("id")] public int Id { get; init; }

    [Column("recipe_id")] public int RecipeId { get; init; }

    [Column("step_number")] public int StepNumber { get; set; }

    [Column("description")] public string Description { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }
}