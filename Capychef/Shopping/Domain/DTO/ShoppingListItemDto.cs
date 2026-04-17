using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.DTO;

public class ShoppingListItemDto
{
    public ShoppingListItemDto(ShoppingListItem item)
    {
        Id = item.Id;
        HouseholdId = item.HouseholdId;
        FoodId = item.FoodId;
        Name = item.Name;
        Quantity = item.Quantity;
        FoodUoMId = item.FoodUoMId;
        PreferredSupermarketId = item.PreferredSupermarketId;
        IsPurchased = item.IsPurchased;
        PurchasedAt = item.PurchasedAt;
        PurchasedBy = item.PurchasedBy;
        IsStored = item.IsStored;
        StoredAt = item.StoredAt;
        StoredBy = item.StoredBy;
        CreatedAt = item.CreatedAt;
        CreatedBy = item.CreatedBy;
        RowVersion = item.RowVersion;
    }

    public int Id { get; init; }

    public int HouseholdId { get; init; }

    public int? FoodId { get; init; }

    public string? Name { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public int? PreferredSupermarketId { get; init; }

    public bool IsPurchased { get; init; }

    public DateTime? PurchasedAt { get; init; }

    public int? PurchasedBy { get; init; }

    public bool IsStored { get; init; }

    public DateTime? StoredAt { get; init; }

    public int? StoredBy { get; init; }

    public DateTime CreatedAt { get; init; }

    public int CreatedBy { get; init; }

    public int RowVersion { get; init; }
}