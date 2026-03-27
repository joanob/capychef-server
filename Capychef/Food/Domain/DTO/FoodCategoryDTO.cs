using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodCategoryDto
{
    public FoodCategoryDto(FoodCategory foodCategory)
    {
        Id = foodCategory.Id;
        Name = foodCategory.Name;
        IsLeaf = foodCategory.IsLeaf;
        ParentCategoryId = foodCategory.ParentCategoryId;
    }

    public int Id { get; }
    public string Name { get; }
    public bool IsLeaf { get; }
    public int? ParentCategoryId { get; }
    public List<FoodCategoryDto>? Children { get; private set; }

    public void AddChildren(List<FoodCategory> categories)
    {
        if (categories.Count > 0)
        {
            Children = categories.Where(x => x.ParentCategoryId == Id).Select(x => new FoodCategoryDto(x)).ToList();
            foreach (var child in Children) child.AddChildren(categories);
        }
    }
}