using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Recipes.Domain.Cmd;

public class RecipeHouseholdDetailsCmd : ICmd
{
    public bool? IsFavourite { get; init; }

    public int? Score { get; init; }

    public int? MinDaysBetweenConsumptions { get; init; }

    public int? MaxDaysBetweenConsumptions { get; init; }

    public ValidationError? Validate()
    {
        if (Score.HasValue && (Score < 0 || Score > 10))
            return new ValidationError("'score' must be between 0 and 10.");

        if (MinDaysBetweenConsumptions.HasValue && MinDaysBetweenConsumptions < 0)
            return new ValidationError("'minDaysBetweenConsumptions' must be greater than or equal to 0.");

        if (MaxDaysBetweenConsumptions.HasValue && MaxDaysBetweenConsumptions < 0)
            return new ValidationError("'maxDaysBetweenConsumptions' must be greater than or equal to 0.");

        if (MinDaysBetweenConsumptions.HasValue && MaxDaysBetweenConsumptions.HasValue &&
            MinDaysBetweenConsumptions > MaxDaysBetweenConsumptions)
            return new ValidationError(
                "'minDaysBetweenConsumptions' cannot be greater than 'maxDaysBetweenConsumptions'.");

        return null;
    }
}