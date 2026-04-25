using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Auth;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes")]
public class Recipe
{
    protected Recipe()
    {
        Name = "";
    }

    [Column("id")] public int Id { get; init; }

    [Column("recipe_type")] public RecipeType Type { get; set; } = RecipeType.Draft;

    [Column("household_id")] public int? HouseholdId { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("description")]
    [MaxLength(5000)]
    public string? Description { get; set; }

    [Column("difficulty")] public int Difficulty { get; set; }

    [Column("cooking_time_minutes")] public int CookingTimeMinutes { get; set; }

    [Column("servings")] public int Servings { get; set; }


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

    public static Recipe NewGlobalRecipe(string name, string? description, int difficulty, int cookingTimeMinutes,
        int servings)
    {
        return new Recipe
        {
            Type = RecipeType.Global,
            Name = name,
            Description = description,
            Difficulty = difficulty,
            CookingTimeMinutes = cookingTimeMinutes,
            Servings = servings,
            HouseholdId = null,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Recipe NewHouseholdRecipe(AuthUserDetails userDetails, string name, string? description,
        int difficulty, int cookingTimeMinutes,
        int servings)
    {
        return new Recipe
        {
            Type = RecipeType.Household,
            HouseholdId = userDetails.GetHouseholdId(),
            Name = name,
            Description = description,
            Difficulty = difficulty,
            CookingTimeMinutes = cookingTimeMinutes,
            Servings = servings,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userDetails.UserId
        };
    }

    public static Recipe NewPublicRecipeDraft(Recipe recipe, AuthUserDetails userDetails)
    {
        return new Recipe
        {
            Type = RecipeType.Draft,
            HouseholdId = userDetails.GetHouseholdId(),
            Name = recipe.Name,
            Description = recipe.Description,
            Difficulty = recipe.Difficulty,
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Servings = recipe.Servings,
            HouseholdRecipeId = recipe.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userDetails.UserId
        };
    }

    public static Recipe NewPublicRecipe(Recipe recipe)
    {
        return new Recipe
        {
            Type = RecipeType.Public,
            Name = recipe.Name,
            Description = recipe.Description,
            Difficulty = recipe.Difficulty,
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Servings = recipe.Servings,
            HouseholdRecipeId = recipe.Id,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .Property(f => f.Type)
            .HasConversion(new RecipeTypeConverter());
    }
}