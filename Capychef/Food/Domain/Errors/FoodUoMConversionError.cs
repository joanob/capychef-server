using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

public class FoodUoMConversionError : ValidationError
{
    public FoodUoMConversionError(int foodId, string uom) : base("")
    {
        Message = "food " + foodId + " uom " + uom + " conversion data was not valid";
    }
}