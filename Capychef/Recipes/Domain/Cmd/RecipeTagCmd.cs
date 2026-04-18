using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Recipes.Domain.Cmd;

public class RecipeTagCmd : ICmd
{
    public required string Tag { get; init; }

    public int OrderNum { get; init; }

    public ValidationError? Validate()
    {
        if (Tag.Length > 50) return new ValidationError("Tag cannot be longer than 50 characters.");

        return null;
    }
}