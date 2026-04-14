using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.DTO;

public class SupermarketDto
{
    public SupermarketDto(Supermarket supermarket)
    {
        Id = supermarket.Id;
        Name = supermarket.Name;
        IsGlobal = supermarket.IsGlobal;
        GlobalId = supermarket.GlobalId;
        HouseholdId = supermarket.HouseholdId;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public bool IsGlobal { get; init; }

    public string? GlobalId { get; init; }

    public int? HouseholdId { get; init; }
}