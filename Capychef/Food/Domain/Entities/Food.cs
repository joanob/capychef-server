using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using YourOwnBoss.Common.Entities;

namespace Capychef.Food.Domain.Entities;

[Table("food")]
public class Food : BaseDeletableEntity
{
    public Food()
    {
    }

    public Food(string globalId, string name, int categoryId)
    {
        Name = name;
        CategoryId = categoryId;
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

    public Food(int householdId, int modifiedGlobalFoodId, string name, int categoryId, int createdBy)
    {
        Name = name;
        CategoryId = categoryId;
        IsGlobal = false;
        HouseholdId = householdId;
        ModifiedGlobalFoodId = modifiedGlobalFoodId;
        CreatedBy = createdBy;
    }

    [Column("name")] public string Name { get; set; }

    [Column("category_id")]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

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

    public Household? Household { get; private set; }

    public Food? ModifiedGlobalFood { get; private set; }

    public User? CreatedByUser { get; private set; }
}