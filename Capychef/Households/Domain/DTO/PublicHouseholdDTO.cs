using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class PublicHouseholdDto
{
    public PublicHouseholdDto(Household household)
    {
        Id = household.Id;
        Name = household.Name;
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public static List<PublicHouseholdDto> ToDtoList(List<Household> households,
        Dictionary<int, int>? memberCounts = null)
    {
        return households.Select(h => new PublicHouseholdDto(h)).ToList();
    }
}