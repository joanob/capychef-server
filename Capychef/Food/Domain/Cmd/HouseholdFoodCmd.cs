using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Food.Domain.Cmd;

public class HouseholdFoodCmd : ICmd
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public double? MinQuantity { get; set; }
    public string? MinQuantityUoM { get; set; }
    public int? DaysUntilExpiration { get; set; }
    public int? DaysUntilBestBefore { get; set; }
    public List<FoodUoMCmd> UoM { get; set; }

    public ValidationError? Validate()
    {
        if (MinQuantity.HasValue && string.IsNullOrEmpty(MinQuantityUoM))
            return new ValidationError("MinQuantityUoM is required when MinQuantity is provided.");

        return null;
    }
}