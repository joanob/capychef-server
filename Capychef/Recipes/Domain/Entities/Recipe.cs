using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes")]
public class Recipe
{
    protected Recipe()
    {
        Name = "";
    }

    public Recipe(string name, string? description, int difficulty, int cookingTimeMinutes, int servings,
        int householdId, int createdBy)
    {
        Name = name;
        Description = description;
        Difficulty = difficulty;
        CookingTimeMinutes = cookingTimeMinutes;
        Servings = servings;
        HouseholdId = householdId;
        CreatedBy = createdBy;
        IsGlobal = false;
        CreatedAt = DateTime.UtcNow;
        RowVersion = 1;
        IsDeleted = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("description")]
    [MaxLength(5000)]
    public string? Description { get; set; }

    [Column("difficulty")] public int Difficulty { get; set; }

    [Column("cooking_time_minutes")] public int CookingTimeMinutes { get; set; }

    [Column("servings")] public int Servings { get; set; }

    [Column("is_global")] public bool IsGlobal { get; init; }

    [Column("household_id")] public int? HouseholdId { get; init; }

    [Column("publication_status")] public RecipePublicationStatus? PublicationStatus { get; set; }

    [Column("private_recipe_id")] public int? PrivateRecipeId { get; init; }

    [Column("public_recipe_id")] public int? PublicRecipeId { get; init; }

    [Column("reviewed_by")] public int? ReviewedBy { get; set; }

    [Column("reviewed_at")] public DateTime? ReviewedAt { get; set; }

    [Column("review_message")]
    [MaxLength(5000)]
    public string? ReviewMessage { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int? CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    public static Recipe CreateDraft(Recipe privateRecipe, int? publicRecipeId, int createdBy)
    {
        return new Recipe
        {
            Name = privateRecipe.Name,
            Description = privateRecipe.Description,
            Difficulty = privateRecipe.Difficulty,
            CookingTimeMinutes = privateRecipe.CookingTimeMinutes,
            Servings = privateRecipe.Servings,
            IsGlobal = false,
            HouseholdId = privateRecipe.HouseholdId,
            PublicationStatus = RecipePublicationStatus.Pending,
            PrivateRecipeId = privateRecipe.Id,
            PublicRecipeId = publicRecipeId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            RowVersion = 1,
            IsDeleted = false
        };
    }

    public static Recipe CreatePublic(Recipe draft, int createdBy)
    {
        return new Recipe
        {
            Name = draft.Name,
            Description = draft.Description,
            Difficulty = draft.Difficulty,
            CookingTimeMinutes = draft.CookingTimeMinutes,
            Servings = draft.Servings,
            IsGlobal = true,
            PublicationStatus = RecipePublicationStatus.Active,
            PrivateRecipeId = draft.PrivateRecipeId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            RowVersion = 1,
            IsDeleted = false
        };
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}