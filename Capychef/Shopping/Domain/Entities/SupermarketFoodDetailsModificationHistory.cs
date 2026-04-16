using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Shopping.Domain.Entities;

[Table("supermarket_food_details_modifications_history")]
public class SupermarketFoodDetailsModificationHistory
{
    protected SupermarketFoodDetailsModificationHistory()
    {
    }

    public SupermarketFoodDetailsModificationHistory(
        int supermarketFoodDetailsId,
        double? price,
        double? quantity,
        int? foodUoMId,
        bool isPreferredSupermarket,
        int createdBy)
    {
        SupermarketFoodDetailsId = supermarketFoodDetailsId;
        Price = price;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        IsPreferredSupermarket = isPreferredSupermarket;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    [Column("id")] public int Id { get; init; }

    [Column("supermarket_food_details_id")]
    public int SupermarketFoodDetailsId { get; init; }

    [Column("price")] public double? Price { get; init; }

    [Column("quantity")] public double? Quantity { get; init; }

    [Column("food_uom_id")] public int? FoodUoMId { get; init; }

    [Column("is_preferred_supermarket")] public bool IsPreferredSupermarket { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }
}

public static class SupermarketFoodDetailsModificationHistoryExtensions
{
    public static IQueryable<SupermarketFoodDetailsModificationHistory> ByDetails(
        this IQueryable<SupermarketFoodDetailsModificationHistory> history,
        int supermarketFoodDetailsId)
    {
        return history.Where(x => x.SupermarketFoodDetailsId == supermarketFoodDetailsId);
    }

    public static IOrderedQueryable<SupermarketFoodDetailsModificationHistory> OrderByNewest(
        this IQueryable<SupermarketFoodDetailsModificationHistory> history)
    {
        return history.OrderByDescending(x => x.CreatedAt);
    }
}