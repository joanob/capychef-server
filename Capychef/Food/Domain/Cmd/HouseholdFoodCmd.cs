using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Food.Domain.Cmd;

public class HouseholdFoodCmd : ICmd
{
    public required string Name { get; init; }
    public required int CategoryId { get; init; }
    public double? MinQuantity { get; init; }
    public string? MinQuantityUoM { get; init; }
    public int? DaysUntilExpiration { get; init; }
    public int? DaysUntilBestBefore { get; init; }
    public required List<FoodUoMCmd> UoM { get; init; }

    public ValidationError? Validate()
    {
        if (MinQuantity.HasValue && string.IsNullOrEmpty(MinQuantityUoM))
            return new ValidationError("MinQuantityUoM is required when MinQuantity is provided.");

        return null;
    }
}