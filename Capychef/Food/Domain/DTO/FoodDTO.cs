namespace Capychef.Food.Domain.DTO;

public class FoodDto
{
    public FoodDto(Entities.Food food)
    {
        Id = food.Id;
        Name = food.Name;
        CategoryId = food.CategoryId;
        MinQuantity = food.HouseholdFoodDetails?.MinQuantity;
        MinQuantityUoM = food.HouseholdFoodDetails?.MinQuantityUoM;
        DaysUntilExpiration = food.GetDaysUntilExpiration();
        DaysUntilBestBefore = food.GetDaysUntilBestBefore();
        IsGlobal = food.IsGlobal;
        HouseholdId = food.HouseholdId;
        ModifiedGlobalFoodId = food.ModifiedGlobalFoodId;
        CreatedBy = food.CreatedBy;

        if (food.UoM.Count > 0)
            UoM = food.UoM.Select(x => new FoodUoMdto(x)).ToList();
        else
            UoM = new List<FoodUoMdto>();
    }

    public int Id { get; }

    public string Name { get; }

    public int CategoryId { get; }

    public double? MinQuantity { get; }

    public string? MinQuantityUoM { get; }

    public int? DaysUntilExpiration { get; set; }

    public int? DaysUntilBestBefore { get; set; }

    public bool IsGlobal { get; }

    public int? HouseholdId { get; }

    public int? ModifiedGlobalFoodId { get; }

    public int? CreatedBy { get; }

    public ICollection<FoodUoMdto> UoM { get; }

    public static List<FoodDto> ToList(List<Entities.Food> food)
    {
        return food.Select(x => new FoodDto(x)).ToList();
    }
}