using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.DTO;

public class RecipeStepDto
{
    public RecipeStepDto(RecipeStep step)
    {
        Id = step.Id;
        RecipeId = step.RecipeId;
        StepNumber = step.StepNumber;
        Description = step.Description;
        CreatedAt = step.CreatedAt;
        CreatedBy = step.CreatedBy;
    }

    public int Id { get; init; }
    public int RecipeId { get; init; }
    public int StepNumber { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public int CreatedBy { get; init; }
}