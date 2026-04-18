using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Food.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Shopping.Domain.Entities;

[Table("shopping_list_items")]
public class ShoppingListItem
{
    protected ShoppingListItem()
    {
    }

    public ShoppingListItem(int? foodId, string? name, double? quantity, int? foodUoMId, int? preferredSupermarketId,
        int householdId, int createdBy)
    {
        FoodId = foodId;
        Name = name;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        PreferredSupermarketId = preferredSupermarketId;
        HouseholdId = householdId;
        CreatedBy = createdBy;
        IsPurchased = false;
        IsStored = false;
        CreatedAt = DateTime.UtcNow;
        RowVersion = 1;
        IsDeleted = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("food_id")] public int? FoodId { get; set; }

    [Column("name")] [MaxLength(50)] public string? Name { get; set; }

    [Column("quantity")] public double? Quantity { get; set; }

    [Column("food_uom_id")] public int? FoodUoMId { get; set; }

    [Column("preferred_supermarket_id")] public int? PreferredSupermarketId { get; set; }

    [Column("is_purchased")] public bool IsPurchased { get; private set; }

    [Column("purchased_at")] public DateTime? PurchasedAt { get; private set; }

    [Column("purchased_by")] public int? PurchasedBy { get; private set; }

    [Column("is_stored")] public bool IsStored { get; private set; }

    [Column("stored_at")] public DateTime? StoredAt { get; private set; }

    [Column("stored_by")] public int? StoredBy { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [InverseProperty(nameof(FoodUoM.Food))]
    public ICollection<FoodUoM> UoM { get; } = new List<FoodUoM>();

    public void Purchase(int userId)
    {
        IsPurchased = true;
        PurchasedAt = DateTime.UtcNow;
        PurchasedBy = userId;
    }

    public void Unpurchase()
    {
        IsPurchased = false;
        PurchasedAt = null;
        PurchasedBy = null;
    }

    public void Store(int userId)
    {
        IsStored = true;
        StoredAt = DateTime.UtcNow;
        StoredBy = userId;
    }

    public void Update(int? foodId, string? name, double? quantity, int? foodUoMId, int? preferredSupermarketId)
    {
        FoodId = foodId;
        Name = name;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        PreferredSupermarketId = preferredSupermarketId;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}

public static class ShoppingListItemExtensions
{
    public static IQueryable<SupermarketFoodDetails> Active(this IQueryable<SupermarketFoodDetails> foodDetails)
    {
        return foodDetails.Where(x => !x.IsDeleted);
    }

    public static IQueryable<SupermarketFoodDetails> IncludeUoM(this IQueryable<SupermarketFoodDetails> foodDetails,
        int? householdId)
    {
        return foodDetails.Include(x => x.UoM.Where(u => u.HouseholdId == householdId || u.HouseholdId == null));
    }
}