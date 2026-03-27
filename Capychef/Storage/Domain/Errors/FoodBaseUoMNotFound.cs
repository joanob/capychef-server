using Capychef.Common.Errors;

namespace Capychef.Storage.Domain.Errors;

public class BatchDoesNotHaveEnoughQuantityError : AppError
{
    public BatchDoesNotHaveEnoughQuantityError(int batchId, double batchQuantity, double requiredQuantity) : base(
        ErrorType.CannotCreate)
    {
        Message = "batch " + batchId + " does not have enough quantity: has " + batchQuantity + ", required " +
                  requiredQuantity;
    }
}