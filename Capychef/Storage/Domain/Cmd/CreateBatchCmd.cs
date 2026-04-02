using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Storage.Domain.Cmd;

public class CreateBatchCmd : ICmd
{
    public int FoodId { get; set; }
    public int StorageSpaceId { get; set; }
    public double Quantity { get; set; }
    public int FoodUoMId { get; set; }
    public DateTime? BestBeforeDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

    public ValidationError? Validate()
    {
        if (Quantity < 0) return new ValidationError("Quantity must be greater than or equal to 0");

        return null;
    }
}