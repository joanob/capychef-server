using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Shopping.Domain.Cmd;

public class SupermarketFoodDetailsCmd : ICmd
{
    public required int SupermarketId { get; init; }

    public required double Price { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public bool IsPrefferedSupermarket { get; init; }

    public ValidationError? Validate()
    {
        if (Price <= 0)
            return new ValidationError("Price must be greater than 0.");

        if (Quantity.HasValue && !FoodUoMId.HasValue)
            return new ValidationError("FoodUoMId must be provided if Quantity is specified.");

        return null;
    }
}