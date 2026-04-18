using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Recipes.Domain.Cmd;

public class RecipeIngredientCmd : ICmd
{
    public required int FoodId { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public int OrderNum { get; init; }

    public ValidationError? Validate()
    {
        if (Quantity.HasValue && !FoodUoMId.HasValue)
            return new ValidationError("'foodUoMId' must be provided if 'quantity' is specified.");

        if (FoodUoMId.HasValue && !Quantity.HasValue)
            return new ValidationError("'quantity' must be provided if 'foodUoMId' is specified.");

        return null;
    }
}