using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("food_categories")]
public class FoodCategory
{
    public FoodCategory()
    {
    }

    public FoodCategory(int id, string name, int? parentCategoryId)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
    }

    [Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("parent_category_id")]
    [ForeignKey(nameof(ParentCategory))]
    public int? ParentCategoryId { get; set; }

    public FoodCategory? ParentCategory { get; private set; }
}