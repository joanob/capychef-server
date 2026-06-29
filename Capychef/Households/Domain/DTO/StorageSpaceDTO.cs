using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class StorageSpaceDto
{
    public StorageSpaceDto(StorageSpace storageSpace)
    {
        Id = storageSpace.Id;
        Name = storageSpace.Name;
        StorageCondition = storageSpace.StorageCondition.ToString();
        RowVersion = storageSpace.RowVersion;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string StorageCondition { get; set; }
    public int RowVersion { get; set; }

    public static List<StorageSpaceDto> ToDtoList(List<StorageSpace> storageSpaces)
    {
        return storageSpaces.Select(s => new StorageSpaceDto(s)).ToList();
    }
}