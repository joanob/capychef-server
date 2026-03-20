using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Domain.Entities;

[Table("food")]
public class Food : BaseDeletableEntity
{
    public Food()
    {
    }

    public Food(string globalId, string name, int categoryId, string baseUoMCode, int? daysUntilExpiration,
        int? daysUntilBestBefore)
    {
        Name = name;
        CategoryId = categoryId;
        BaseUoM = baseUoMCode;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
        IsGlobal = true;
        GlobalId = globalId;
    }

    public Food(int householdId, string name, int categoryId, string baseUoMCode, int createdBy,
        int? daysUntilExpiration, int? daysUntilBestBefore)
    {
        Name = name;
        CategoryId = categoryId;
        BaseUoM = baseUoMCode;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
        IsGlobal = false;
        HouseholdId = householdId;
        CreatedBy = createdBy;
    }

    public Food(int householdId, int modifiedGlobalFoodId, string name, int categoryId, string baseUoMCode,
        int createdBy, int? daysUntilExpiration, int? daysUntilBestBefore)
    {
        Name = name;
        CategoryId = categoryId;
        BaseUoM = baseUoMCode;
        DaysUntilExpiration = daysUntilExpiration;
        DaysUntilBestBefore = daysUntilBestBefore;
        IsGlobal = false;
        HouseholdId = householdId;
        ModifiedGlobalFoodId = modifiedGlobalFoodId;
        CreatedBy = createdBy;
    }

    [Column("name")] public string Name { get; set; }

    [Column("category_id")]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [Column("base_uom")]
    [ForeignKey(nameof(BaseUoMInstance))]
    public string BaseUoM { get; set; }

    [Column("days_until_expiration")] public int? DaysUntilExpiration { get; set; }

    [Column("days_until_best_before")] public int? DaysUntilBestBefore { get; set; }

    [Column("is_global")] public bool IsGlobal { get; private set; }

    [Column("global_id")] public string? GlobalId { get; private set; }

    [Column("household_id")]
    [ForeignKey(nameof(Household))]
    public int? HouseholdId { get; private set; }

    [Column("modified_global_food_id")]
    [ForeignKey(nameof(ModifiedGlobalFood))]
    public int? ModifiedGlobalFoodId { get; private set; }

    [Column("created_by")]
    [ForeignKey(nameof(CreatedByUser))]
    public int? CreatedBy { get; private set; }

    public FoodCategory Category { get; set; }

    public UoM BaseUoMInstance { get; set; }

    public Household? Household { get; private set; }

    public Food? ModifiedGlobalFood { get; private set; }

    public User? CreatedByUser { get; private set; }

    [InverseProperty(nameof(FoodUoM.Food))]
    public ICollection<FoodUoM> UoM { get; } = new List<FoodUoM>();

    public void AddUoM(string uom)
    {
        UoM.Add(new FoodUoM(this, uom, null, null, null));
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
        BaseUoM = food.BaseUoM;
        DaysUntilExpiration = food.DaysUntilExpiration;
        DaysUntilBestBefore = food.DaysUntilBestBefore;
    }
}

public static class FoodExtensions
{
    public static IQueryable<Food> Active(this IQueryable<Food> food)
    {
        return food.Where(x => !x.IsDeleted);
    }

    public static IQueryable<Food> IncludeUoM(this IQueryable<Food> food)
    {
        return food.Include(x => x.UoM);
    }
}