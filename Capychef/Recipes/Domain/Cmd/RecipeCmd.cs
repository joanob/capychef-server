using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Recipes.Domain.Cmd;

public class RecipeCmd : ICmd
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required int Difficulty { get; init; }

    public required int CookingTimeMinutes { get; init; }

    public required int Servings { get; init; }

    public ValidationError? Validate()
    {
        if (Name.Length > 50) return new ValidationError("Name cannot be longer than 50 characters.");
        if (Difficulty is < 1 or > 5) return new ValidationError("Difficulty must be between 1 and 5.");
        if (CookingTimeMinutes <= 0) return new ValidationError("CookingTimeMinutes must be greater than 0.");
        if (Servings <= 0) return new ValidationError("Servings must be greater than 0.");

        return null;
    }
}