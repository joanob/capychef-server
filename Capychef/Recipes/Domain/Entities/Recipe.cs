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

    [Column("is_public")] public bool IsPublic { get; set; }

    [Column("published_at")] public DateTime? PublishedAt { get; set; }

    [Column("published_by")] public int? PublishedBy { get; set; }

    [Column("household_recipe_id")] public int? HouseholdRecipeId { get; init; }

    [Column("reviewed_at")] public DateTime? ReviewedAt { get; set; }

    [Column("reviewed_by")] public int? ReviewedBy { get; set; }

    [Column("review_message")]
    [MaxLength(5000)]
    public string? ReviewMessage { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int? CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public static Recipe CreatePublicationDraft(Recipe recipe, int userId)
    {
        return new Recipe
        {
            Name = recipe.Name,
            Description = recipe.Description,
            Difficulty = recipe.Difficulty,
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Servings = recipe.Servings,
            IsGlobal = false,
            HouseholdId = recipe.HouseholdId,
            IsPublic = false,
            HouseholdRecipeId = recipe.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}