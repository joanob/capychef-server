using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

/**
 * Food uom list does not have any base uom
 */
public class FoodBaseUoMNotFound : NotFoundError
{
    public FoodBaseUoMNotFound(int foodId) : base(EntityType.FoodBaseUoM, foodId)
    {
        Message = "food " + foodId + " does not have a base uom";
    }
}