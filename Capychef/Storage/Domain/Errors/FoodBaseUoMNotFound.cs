using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

public class BatchDoesNotHaveEnoughQuantityError : AppError
{
    public BatchDoesNotHaveEnoughQuantityError(int batchId, double batchQuantity, double requiredQuantity) : base(
        ErrorType.CANNOT_CREATE, "")
    {
        _message = "batch " + batchId + " does not have enough quantity: has " + batchQuantity + ", required " +
                   requiredQuantity;
    }
}