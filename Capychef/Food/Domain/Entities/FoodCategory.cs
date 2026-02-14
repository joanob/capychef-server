using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("food_categories")]
public class FoodCategory
{
    public FoodCategory()
    {
    }

    public FoodCategory(int id, string name, bool isLeaf, int? parentCategoryId)
    {
        Id = id;
        Name = name;
        IsLeaf = isLeaf;
        ParentCategoryId = parentCategoryId;
    }

    [Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    // Leaf categories are those that do not have any subcategories 
    // Food always belongs to leaf categories
    [Column("is_leaf")] public bool IsLeaf { get; set; }

    [Column("parent_category_id")]
    [ForeignKey(nameof(ParentCategory))]
    public int? ParentCategoryId { get; set; }

    public FoodCategory? ParentCategory { get; private set; }
}