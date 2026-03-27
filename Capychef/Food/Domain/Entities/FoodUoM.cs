using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Households.Domain.Entities;

namespace Capychef.Food.Domain.Entities;

[Table("food_uom")]
public class FoodUoM : BaseDeletableEntity
{
    public FoodUoM(int foodId, int? householdId, string uoM, bool isBaseUoM, int? numerator, int? denominator,
        bool? isApproxConversion)
    {
        FoodId = foodId;
        HouseholdId = householdId;
        UoM = uoM;
        IsBaseUoM = isBaseUoM;
        Numerator = numerator;
        Denominator = denominator;
        IsApproxConversion = isApproxConversion;
    }

    public FoodUoM(Food food, int? householdId, string uoM, bool isBaseUoM, int? numerator, int? denominator,
        bool? isApproxConversion)
    {
        Food = food;
        HouseholdId = householdId;
        IsBaseUoM = isBaseUoM;
        UoM = uoM;
        Numerator = numerator;
        Denominator = denominator;
        IsApproxConversion = isApproxConversion;
    }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("uom")] [MaxLength(4)] public string UoM { get; }

    [Column("household_id")] public int? HouseholdId { get; }

    [Column("is_base")] public bool IsBaseUoM { get; set; }

    [Column("base_uom")] public string? BaseUoM { get; init; }

    [Column("numerator")] public int? Numerator { get; private set; }

    [Column("denominator")] public int? Denominator { get; private set; }

    [Column("is_approx_conversion")] public bool? IsApproxConversion { get; set; }
    [ForeignKey(nameof(FoodId))] public Food? Food { get; private set; }

    [ForeignKey(nameof(UoM))] public UoM? UoMInstance { get; init; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(BaseUoM))] public UoM? BaseUoMInstance { get; init; }

    public void Set(bool isBaseUoM, int? numerator, int? denominator, bool? isApproxConversion)
    {
        IsBaseUoM = isBaseUoM;
        Numerator = numerator;
        Denominator = denominator;
        IsApproxConversion = isApproxConversion;
    }

    public void RestoreDeleted()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}

public static class FoodUoMExtensions
{
    public static IQueryable<FoodUoM> Active(this IQueryable<FoodUoM> foodUoMs)
    {
        return foodUoMs.Where(x => !x.IsDeleted);
    }

    public static IQueryable<FoodUoM> FromHousehold(this IQueryable<FoodUoM> foodUoMs, int? householdId)
    {
        if (householdId == null)
            return foodUoMs.Where(x => x.HouseholdId == null);

        return foodUoMs
            .Where(x => x.HouseholdId == null || x.HouseholdId == householdId)
            .Where(x => x.HouseholdId == householdId
                        || !foodUoMs.Any(y => y.FoodId == x.FoodId
                                              && y.UoM == x.UoM
                                              && y.HouseholdId == householdId
                                              && !y.IsDeleted));
    }
}