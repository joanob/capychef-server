namespace Capychef.Storage.Domain.Cmd;

public class CreateBatchCmd
{
    public int FoodId { get; set; }
    public int StorageSpaceId { get; set; }
    public double Quantity { get; set; }
    public int FoodUoMId { get; set; }
}