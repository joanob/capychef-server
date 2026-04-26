using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.DTO;

public class RecipeHouseholdDetailsDto
{
    public RecipeHouseholdDetailsDto(RecipeHouseholdDetails details)
    {
        Id = details.Id;
        RecipeId = details.RecipeId;
        HouseholdId = details.HouseholdId;
        UserId = details.UserId;
        IsFavourite = details.IsFavourite;
        Score = details.Score;
        MinDaysBetweenConsumptions = details.MinDaysBetweenConsumptions;
        MaxDaysBetweenConsumptions = details.MaxDaysBetweenConsumptions;
        CreatedAt = details.CreatedAt;
        CreatedBy = details.CreatedBy;
    }

    public int Id { get; init; }
    public int RecipeId { get; init; }
    public int HouseholdId { get; init; }
    public int? UserId { get; init; }
    public bool IsFavourite { get; init; }
    public int Score { get; init; }
    public int? MinDaysBetweenConsumptions { get; init; }
    public int? MaxDaysBetweenConsumptions { get; init; }
    public DateTime CreatedAt { get; init; }
    public int CreatedBy { get; init; }
}