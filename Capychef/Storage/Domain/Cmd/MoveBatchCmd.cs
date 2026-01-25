namespace Capychef.Storage.Domain.Cmd;

public class MoveBatchCmd
{
    public int StorageSpaceId { get; set; }
    public double Quantity { get; set; }
}