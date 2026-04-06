using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

/// <summary>
///     The food does not have a uom with the given id
/// </summary>
public class FoodUoMNotFound : NotFoundError
{
    public FoodUoMNotFound(int foodId, int foodUoMId) : base(EntityType.FoodUoM, foodUoMId)
    {
        Message = "food " + foodId + " does not have a uom with id " + foodUoMId;
    }
}