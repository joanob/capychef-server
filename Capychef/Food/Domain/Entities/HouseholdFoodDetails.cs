using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;

namespace Capychef.Food.Domain.Entities;

[Table("household_food_details")]
public class HouseholdFoodDetails
{
    protected HouseholdFoodDetails()
    {
    }

    public HouseholdFoodDetails(int householdId, Food food, double? minQuantity, string? minQuantityUoM,
        int? daysUntilExpiration, int? daysUntilBestBefore)
    {
        HouseholdId = householdId;
        Food = food;
        MinQuantity = minQuantity;
        MinQuantityUoM = minQuantityUoM;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
    }

    public HouseholdFoodDetails(int householdId, int foodId, double? minQuantity, string? minQuantityUoM,
        int? daysUntilExpiration, int? daysUntilBestBefore)
    {
        HouseholdId = householdId;
        FoodId = foodId;
        MinQuantity = minQuantity;
        MinQuantityUoM = minQuantityUoM;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("min_quantity")] public double? MinQuantity { get; set; }

    [Column("min_quantity_uom")]
    [MaxLength(4)]
    public string? MinQuantityUoM { get; set; }

    [Column("days_until_expiration")] public int? DaysUntilExpiration { get; set; }

    [Column("days_until_best_before")] public int? DaysUntilBestBefore { get; set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(FoodId))] public Food? Food { get; init; }
}