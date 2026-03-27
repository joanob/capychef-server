namespace Capychef.Households.Domain.Cmd;

public class StorageSpaceCmd
{
    public required string Name { get; init; }

    public required string StorageCondition { get; init; }
}