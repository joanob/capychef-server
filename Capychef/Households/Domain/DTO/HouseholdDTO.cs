using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class HouseholdDto
{
    public HouseholdDto(Household household, bool includeStorageSpaces = false)
    {
        Id = household.Id;
        OwnerId = household.OwnerId;
        Name = household.Name;

        StorageSpaces = includeStorageSpaces
            ? household.StorageSpaces.Select(x => new StorageSpaceDto(x)).ToList()
            : null;
    }

    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public List<StorageSpaceDto>? StorageSpaces { get; set; }

    public static List<HouseholdDto> ToDtoList(List<Household> households)
    {
        return households.Select(h => new HouseholdDto(h)).ToList();
    }
}