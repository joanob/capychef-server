namespace Capychef.Food.Domain.DTO;

public class FoodDTO
{
    public FoodDTO(Entities.Food food)
    {
        Id = food.Id;
        Name = food.Name;
        CategoryId = food.CategoryId;
        BaseUoM = food.BaseUoM;
        DaysUntilExpiration = food.DaysUntilExpiration;
        DaysUntilBestBefore = food.DaysUntilBestBefore;
        IsGlobal = food.IsGlobal;
        HouseholdId = food.HouseholdId;
        ModifiedGlobalFoodId = food.ModifiedGlobalFoodId;
        CreatedBy = food.CreatedBy;

        if (food.UoM != null)
            UoM = food.UoM.Select(x => new FoodUoMDTO(x)).ToList();
        else
            UoM = new List<FoodUoMDTO>();
    }

    public int Id { get; }

    public string Name { get; }

    public int CategoryId { get; }

    public string BaseUoM { get; }

    public int? DaysUntilExpiration { get; set; }

    public int? DaysUntilBestBefore { get; set; }

    public bool IsGlobal { get; }

    public int? HouseholdId { get; }

    public int? ModifiedGlobalFoodId { get; }

    public int? CreatedBy { get; }

    public ICollection<FoodUoMDTO> UoM { get; }

    public static List<FoodDTO> ToList(List<Entities.Food> food)
    {
        return food.Select(x => new FoodDTO(x)).ToList();
    }
}