using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Recipes.Domain.Cmd;

public class RecipeStepCmd : ICmd
{
    public required string Description { get; init; }

    public int StepNumber { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            return new ValidationError("Description cannot be empty.");

        return null;
    }
}