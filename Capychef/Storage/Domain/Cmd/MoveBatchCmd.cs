namespace Capychef.Storage.Domain.Cmd;

public class MoveBatchCmd : ModifyBatchCmd
{
    public int StorageSpaceId { get; init; }

    // ICmd Validate() method is in ModifyBatchCmd
}