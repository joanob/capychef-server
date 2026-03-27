using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class InitialStorageSpaceDto
{
    public InitialStorageSpaceDto(InitialStorageSpace s)
    {
        Id = s.Id;
        Name = s.Name;
        StorageCondition = s.StorageCondition.ToString();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string StorageCondition { get; set; }

    public static List<InitialStorageSpaceDto> ToDtoList(List<InitialStorageSpace> list)
    {
        return list.Select(x => new InitialStorageSpaceDto(x)).ToList();
    }
}