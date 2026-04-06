using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

/// <summary>
///     A FoodUoM does not have the numerator/denominator data required to perform a conversion.
/// </summary>
public class FoodUoMConversionNotFoundError : NotFoundError
{
    public FoodUoMConversionNotFoundError(int foodId, string uom) : base(EntityType.FoodUoM, uom)
    {
        Message = "food " + foodId + " uom " + uom + " does not have conversion data";
    }
}