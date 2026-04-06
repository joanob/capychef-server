using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Storage.Domain.Cmd;

public class MoveBatchCmd : ICmd
{
    public int StorageSpaceId { get; init; }
    public double Quantity { get; init; }
    public int FoodUoMId { get; init; }
    public double? NewQuantity { get; init; }
    public int? NewFoodUoMId { get; init; }

    public ValidationError? Validate()
    {
        if (NewQuantity == null && NewFoodUoMId == null) return null;

        if (NewQuantity != null && NewFoodUoMId != null) return null;

        if (Quantity < 0) return new ValidationError("Quantity must be greater than or equal to 0");

        return new ValidationError("Both NewQuantity and NewFoodUoMId must be provided together or not at all");
    }
}