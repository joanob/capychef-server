using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("food_categories")]
public class FoodCategory
{
    protected FoodCategory()
    {
        Name = "";
    }

    public FoodCategory(int id, string name, bool isLeaf, int? parentCategoryId)
    {
        Id = id;
        Name = name;
        IsLeaf = isLeaf;
        ParentCategoryId = parentCategoryId;
    }

    [Column("id")] public int Id { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; private set; }

    // Leaf categories are those that do not have any subcategories 
    // Food always belongs to leaf categories
    [Column("is_leaf")] public bool IsLeaf { get; private set; }

    [Column("parent_category_id")] public int? ParentCategoryId { get; set; }

    [ForeignKey(nameof(ParentCategoryId))] public FoodCategory? ParentCategory { get; init; }

    public void Set(FoodCategory foodCategoryLevel1)
    {
        Name = foodCategoryLevel1.Name;
        IsLeaf = foodCategoryLevel1.IsLeaf;
        ParentCategoryId = foodCategoryLevel1.ParentCategoryId;
    }
}