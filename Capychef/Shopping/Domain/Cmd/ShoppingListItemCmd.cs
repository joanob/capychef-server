using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Shopping.Domain.Cmd;

public class ShoppingListItemCmd : ICmd
{
    public int? FoodId { get; init; }

    public string? Name { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public ValidationError? Validate()
    {
        // Al menos name o food_id debe ser indicado pero no ambos
        if ((FoodId.HasValue && !string.IsNullOrEmpty(Name)) ||
            (!FoodId.HasValue && string.IsNullOrEmpty(Name)))
            return new ValidationError("Either 'foodId' or 'name' must be provided, but not both.");

        // Si food_uom_id es proporcionado, quantity debe ser proporcionado
        if (FoodUoMId.HasValue && !Quantity.HasValue)
            return new ValidationError("'quantity' must be provided if 'foodUoMId' is specified.");

        return null;
    }
}