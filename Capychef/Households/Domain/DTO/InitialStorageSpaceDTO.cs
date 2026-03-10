using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class InitialStorageSpaceDTO
{
    public InitialStorageSpaceDTO(InitialStorageSpace s)
    {
        Id = s.Id;
        Name = s.Name;
        StorageCondition = s.StorageCondition.ToString();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string StorageCondition { get; set; }

    public static List<InitialStorageSpaceDTO> ToDTOList(List<InitialStorageSpace> list)
    {
        return list.Select(x => new InitialStorageSpaceDTO(x)).ToList();
    }
}