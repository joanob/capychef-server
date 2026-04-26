using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;

namespace Capychef.Recipes.Domain.Entities;

[Table("recipes_household_details")]
public class RecipeHouseholdDetails
{
    protected RecipeHouseholdDetails()
    {
    }

    public RecipeHouseholdDetails(int recipeId, int householdId, int createdBy)
    {
        RecipeId = recipeId;
        HouseholdId = householdId;
        IsFavourite = false;
        Score = 0;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        RowVersion = 1;
        IsDeleted = false;
    }

    public RecipeHouseholdDetails(int recipeId, int householdId, int? userId, int createdBy)
    {
        RecipeId = recipeId;
        HouseholdId = householdId;
        UserId = userId;
        IsFavourite = false;
        Score = 0;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        RowVersion = 1;
        IsDeleted = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("recipe_id")] public int RecipeId { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("user_id")] public int? UserId { get; init; }

    [Column("is_favourite")] public bool IsFavourite { get; set; }

    [Column("score")] public int Score { get; set; }

    [Column("min_days_between_consumptions")]
    public int? MinDaysBetweenConsumptions { get; set; }

    [Column("max_days_between_consumptions")]
    public int? MaxDaysBetweenConsumptions { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(RecipeId))] public Recipe? Recipe { get; init; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(UserId))] public User? User { get; init; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}