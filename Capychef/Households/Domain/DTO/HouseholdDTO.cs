using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class HouseholdDTO
{
    public HouseholdDTO(Household household)
    {
        Id = household.Id;
        OwnerId = household.OwnerId;
        Name = household.Name;

        if (household.StorageSpaces != null)
            StorageSpaces = household.StorageSpaces.Select(x => new StorageSpaceDTO(x)).ToList();
    }

    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public List<StorageSpaceDTO> StorageSpaces { get; set; }

    public static List<HouseholdDTO> ToDTOList(List<Household> households)
    {
        return households.Select(h => new HouseholdDTO(h)).ToList();
    }
}