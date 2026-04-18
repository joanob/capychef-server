using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Shopping.Domain.Cmd;

public class StoreShoppingListItemCmd : ICmd
{
    public required int StorageSpaceId { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public ValidationError? Validate()
    {
        // Si se indica Quantity, debe indicarse FoodUoMId
        if (Quantity.HasValue && !FoodUoMId.HasValue)
            return new ValidationError("'foodUoMId' must be provided if 'quantity' is specified.");

        // Si se indica FoodUoMId, debe indicarse Quantity
        if (FoodUoMId.HasValue && !Quantity.HasValue)
            return new ValidationError("'quantity' must be provided if 'foodUoMId' is specified.");

        return null;
    }
}