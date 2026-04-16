using Capychef.Shopping.Domain.Entities;

namespace Capychef.Shopping.Domain.DTO;

public class SupermarketFoodDetailsDto
{
    public SupermarketFoodDetailsDto(SupermarketFoodDetails details)
    {
        Id = details.Id;
        HouseholdId = details.HouseholdId;
        FoodId = details.FoodId;
        SupermarketId = details.SupermarketId;
        Price = details.Price;
        Quantity = details.Quantity;
        FoodUoMId = details.FoodUoMId;
        IsPrefferedSupermarket = details.IsPrefferedSupermarket;
        CreatedAt = details.CreatedAt;
        CreatedBy = details.CreatedBy;
        RowVersion = details.RowVersion;
    }

    public int Id { get; init; }

    public int HouseholdId { get; init; }

    public int FoodId { get; init; }

    public int SupermarketId { get; init; }

    public double Price { get; init; }

    public double? Quantity { get; init; }

    public int? FoodUoMId { get; init; }

    public bool IsPrefferedSupermarket { get; init; }

    public DateTime CreatedAt { get; init; }

    public int CreatedBy { get; init; }

    public int RowVersion { get; init; }
}