using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Capychef.Food.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Shopping.Domain.Entities;

[Table("supermarket_food_details")]
public class SupermarketFoodDetails
{
    protected SupermarketFoodDetails()
    {
    }

    public SupermarketFoodDetails(int householdId, int foodId, int supermarketId, double price, double? quantity,
        int? foodUoMId, bool isPrefferedSupermarket, int createdBy)
    {
        HouseholdId = householdId;
        FoodId = foodId;
        SupermarketId = supermarketId;
        Price = price;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        IsPrefferedSupermarket = isPrefferedSupermarket;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("supermarket_id")] public int SupermarketId { get; init; }

    [Column("price")] public double Price { get; set; }

    [Column("quantity")] public double? Quantity { get; set; }

    [Column("food_uom_id")] public int? FoodUoMId { get; set; }

    [Column("is_preffered_supermarket")] public bool IsPrefferedSupermarket { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; init; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(FoodId))] public Food.Domain.Entities.Food Food { get; init; }

    [NotMapped] public IEnumerable<FoodUoM> UoM => Food?.UoM ?? Enumerable.Empty<FoodUoM>();

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}

public static class SupermarketFoodDetailsExtensions
{
    public static IQueryable<SupermarketFoodDetails> Active(this IQueryable<SupermarketFoodDetails> foodDetails)
    {
        return foodDetails.Where(x => !x.IsDeleted);
    }

    public static IQueryable<SupermarketFoodDetails> IncludeUoM(this IQueryable<SupermarketFoodDetails> foodDetails,
        int? householdId)
    {
        return foodDetails.Include(x => x.Food)
            .ThenInclude(f => f.UoM);
    }
}