using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.DTO;

public class RecipeDto
{
    public RecipeDto(Recipe recipe)
    {
        Id = recipe.Id;
        Name = recipe.Name;
        Description = recipe.Description;
        Difficulty = recipe.Difficulty;
        CookingTimeMinutes = recipe.CookingTimeMinutes;
        Servings = recipe.Servings;
        Type = recipe.Type;
        HouseholdId = recipe.HouseholdId;
        CreatedAt = recipe.CreatedAt;
        CreatedBy = recipe.CreatedBy;
        RowVersion = recipe.RowVersion;
    }

    public int Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public int Difficulty { get; init; }
    public int CookingTimeMinutes { get; init; }
    public int Servings { get; init; }
    public RecipeType Type { get; init; }
    public int? HouseholdId { get; init; }
    public DateTime CreatedAt { get; init; }
    public int? CreatedBy { get; init; }
    public int RowVersion { get; init; }
}