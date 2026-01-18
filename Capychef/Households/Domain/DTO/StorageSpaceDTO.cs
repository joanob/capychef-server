using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class StorageSpaceDTO
{
    public StorageSpaceDTO(StorageSpace storageSpace)
    {
        Id = storageSpace.Id;
        Name = storageSpace.Name;
        StorageCondition = storageSpace.StorageCondition.ToString();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string StorageCondition { get; set; }

    public static List<StorageSpaceDTO> ToDTOList(List<StorageSpace> storageSpaces)
    {
        return storageSpaces.Select(s => new StorageSpaceDTO(s)).ToList();
    }
}