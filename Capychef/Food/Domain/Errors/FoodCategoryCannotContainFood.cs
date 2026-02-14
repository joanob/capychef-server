using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Food.Domain.Errors;

public class FoodCategoryCannotContainFood : AppError
{
    public FoodCategoryCannotContainFood(int categoryId) : base(ErrorType.CannotCreate, EntityType.FoodCategory,
        categoryId)
    {
        Message = $"Category {categoryId} cannot contain foodll";
    }
}