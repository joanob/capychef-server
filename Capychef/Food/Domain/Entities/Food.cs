using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Domain.Entities;

[Table("food")]
public class Food
{
    protected Food() {}
    
    public Food(string globalId, string name, int categoryId, int? daysUntilExpiration,
        int? daysUntilBestBefore)
    {
        Name = name;
        CategoryId = categoryId;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
        IsGlobal = true;
        GlobalId = globalId;
    }

    public Food(int householdId, string name, int categoryId, int createdBy)
    {
        Name = name;
        CategoryId = categoryId;
        IsGlobal = false;
        HouseholdId = householdId;
        CreatedBy = createdBy;
    }

    public Food(int householdId, int modifiedGlobalFoodId, string name, int categoryId,
        int createdBy)
    {
        Name = name;
        CategoryId = categoryId;
        IsGlobal = false;
        HouseholdId = householdId;
        ModifiedGlobalFoodId = modifiedGlobalFoodId;
        CreatedBy = createdBy;
    }
    
    [Column("id")] public int Id { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("category_id")] public int CategoryId { get; set; }

    [Column("is_global")] public bool IsGlobal { get; init; }

    [Column("global_id")] [MaxLength(50)] public string? GlobalId { get; init; }

    [Column("household_id")] public int? HouseholdId { get; init; }

    [Column("modified_global_food_id")] public int? ModifiedGlobalFoodId { get; init; }

    [Column("days_until_expiration")] private int? DaysUntilExpiration { get; set; }

    [Column("days_until_best_before")] private int? DaysUntilBestBefore { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } =  DateTime.UtcNow;
    
    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; init; }
    
    [Column("is_deleted")] public bool IsDeleted { get; private set; }
    
    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(CategoryId))] public FoodCategory? Category { get; init; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(ModifiedGlobalFoodId))]
    public Food? ModifiedGlobalFood { get; init; }

    [InverseProperty(nameof(FoodUoM.Food))]
    public ICollection<FoodUoM> UoM { get; private set; } = new List<FoodUoM>();

    [InverseProperty(nameof(HouseholdFoodDetails.Food))]
    public ICollection<HouseholdFoodDetails> HouseholdDetailsCollection { get; private set; } =
        new List<HouseholdFoodDetails>();

    public void SetGlobalDaysUntilExpiration(int? daysUntilExpiration)
    {
        if (!HouseholdId.HasValue)
        {
            DaysUntilExpiration = daysUntilExpiration;
        }
    }
    
    public void SetGlobalDaysUntilBestBefore(int? daysUntilBestBefore)
    {
        if (!HouseholdId.HasValue)
        {
            DaysUntilBestBefore = daysUntilBestBefore;
        }
    }

    public HouseholdFoodDetails? HouseholdFoodDetails => HouseholdDetailsCollection.FirstOrDefault();

    public int? GetDaysUntilBestBefore()
    {
        if (HouseholdId.HasValue) return HouseholdFoodDetails?.DaysUntilBestBefore;

        return DaysUntilBestBefore;
    }

    public int? GetDaysUntilExpiration()
    {
        if (HouseholdId.HasValue) return HouseholdFoodDetails?.DaysUntilExpiration;

        return DaysUntilExpiration;
    }

    public void FromHousehold(int householdId)
    {
        UoM = UoM.Where(x => x.HouseholdId == null || x.HouseholdId == householdId)
            .Where(x => x.HouseholdId == householdId
                        || !UoM.Any(y => y.FoodId == x.FoodId
                                         && y.UoM == x.UoM
                                         && y.HouseholdId == householdId))
            .Where(x => !x.IsDeleted).ToList();
    }

    public void AddUoM(FoodUoM uom)
    {
        UoM.Add(uom);
    }

    public void DeleteUom(string uom)
    {
        var uoM = UoM.FirstOrDefault(x => x.UoM == uom);

        if (uoM != null) UoM.Remove(uoM);
    }

    public void Set(Food food)
    {
        Name = food.Name;
        CategoryId = food.CategoryId;
        DaysUntilBestBefore = food.DaysUntilBestBefore;
        DaysUntilExpiration = food.DaysUntilExpiration;
    }

    public void AddHouseholdFoodDetails(int householdId, double? minQuantity, string? minQuantityUoM,
        int? daysUntilExpiration,
        int? daysUntilBestBefore)
    {
        if (!minQuantity.HasValue && string.IsNullOrEmpty(minQuantityUoM) && !daysUntilExpiration.HasValue &&
            !daysUntilBestBefore.HasValue) return;


        HouseholdDetailsCollection = new List<HouseholdFoodDetails>
            { new(householdId, this, minQuantity, minQuantityUoM, daysUntilExpiration, daysUntilBestBefore) };
    }

    public void SetHouseholdFoodDetails(HouseholdFoodDetails details)
    {
        HouseholdDetailsCollection = new List<HouseholdFoodDetails> { details };
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}

public static class FoodExtensions
{
    public static IQueryable<Food> Active(this IQueryable<Food> food)
    {
        return food.Where(x => !x.IsDeleted);
    }

    public static IQueryable<Food> IncludeUoM(this IQueryable<Food> food, int? householdId)
    {
        return food.Include(x => x.UoM.Where(u => u.HouseholdId == householdId || u.HouseholdId == null));
    }

    public static IQueryable<Food> IncludeHouseholdFoodDetails(this IQueryable<Food> food, int? householdId)
    {
        if (!householdId.HasValue) return food;

        return food.Include(x => x.HouseholdDetailsCollection.Where(h => h.HouseholdId == householdId.Value));
    }
}