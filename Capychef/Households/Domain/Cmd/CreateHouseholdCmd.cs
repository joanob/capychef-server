namespace Capychef.Households.Domain.Cmd;

public class CreateHouseholdCmd
{
    public string Name { get; set; } = string.Empty;
    public List<int> InitialStorageSpaceIds { get; set; } = new();
}