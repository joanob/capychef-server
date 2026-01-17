using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;

namespace Capychef.Users.Domain.Errors;

/**
 * Food base UoM is not in food uom list
 */
public class FoodBaseUoMNotFound : NotFoundError
{
    public FoodBaseUoMNotFound(int foodId, string uom) : base(EntityType.UoM, uom)
    {
        UoM = uom;

        _message = "food " + foodId + " does not have uom" + uom;
    }

    public string UoM { get; }
}