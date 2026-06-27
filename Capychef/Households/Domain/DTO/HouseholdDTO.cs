using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class HouseholdDto
{
    public HouseholdDto(Household household, bool includeStorageSpaces = false, int? memberCount = null)
    {
        Id = household.Id;
        OwnerId = household.OwnerId;
        Name = household.Name;
        MemberCount = memberCount;

        StorageSpaces = includeStorageSpaces
            ? household.StorageSpaces.Select(x => new StorageSpaceDto(x)).ToList()
            : null;
    }

    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public int? MemberCount { get; set; }
    public List<StorageSpaceDto>? StorageSpaces { get; set; }

    public static List<HouseholdDto> ToDtoList(List<Household> households, Dictionary<int, int>? memberCounts = null)
    {
        return households.Select(h => new HouseholdDto(h, memberCount: memberCounts?.GetValueOrDefault(h.Id))).ToList();
    }
}