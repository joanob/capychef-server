using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Storage.Domain.Cmd;

public class UpdateBatchCmd : ICmd
{
    public double Quantity { get; init; }
    public int FoodUoMId { get; init; }
    public DateTime? BestBeforeDate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public bool IsOpen { get; init; }

    public ValidationError? Validate()
    {
        return Quantity < 0 ? new ValidationError("Quantity cannot be negative") : null;
    }
}