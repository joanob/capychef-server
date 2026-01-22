using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.DTO;

public class FoodCategoryDTO
{
    public FoodCategoryDTO(FoodCategory foodCategory)
    {
        Id = foodCategory.Id;
        Name = foodCategory.Name;
        ParentCategoryId = foodCategory.ParentCategoryId;
    }

    public int Id { get; }
    public string Name { get; }
    public int? ParentCategoryId { get; }
    public List<FoodCategoryDTO>? Children { get; private set; }

    public void AddChildren(List<FoodCategory> categories)
    {
        if (categories != null)
        {
            Children = categories.Where(x => x.ParentCategoryId == Id).Select(x => new FoodCategoryDTO(x)).ToList();
            foreach (var child in Children) child.AddChildren(categories);
        }
    }
}