namespace Capychef.Food.Domain.Cmd;

public class FoodDTO
{
    public FoodDTO(Entities.Food food)
    {
        Id = food.Id;
        Name = food.Name;
        CategoryId = food.CategoryId;
        IsGlobal = food.IsGlobal;
        HouseholdId = food.HouseholdId;
        ModifiedGlobalFoodId = food.ModifiedGlobalFoodId;
        CreatedBy = food.CreatedBy;
    }

    public int Id { get; }

    public string Name { get; }

    public int CategoryId { get; }

    public bool IsGlobal { get; }

    public int? HouseholdId { get; }

    public int? ModifiedGlobalFoodId { get; }

    public int? CreatedBy { get; }

    public static List<FoodDTO> ToList(List<Entities.Food> food)
    {
        return food.Select(x => new FoodDTO(x)).ToList();
    }
}