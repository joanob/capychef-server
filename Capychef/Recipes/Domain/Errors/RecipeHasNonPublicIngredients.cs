using Capychef.Common.Errors;

namespace Capychef.Recipes.Domain.Errors;

public class RecipeHasNonPublicIngredients : AppError
{
    public RecipeHasNonPublicIngredients() : base(ErrorType.CannotCreate)
    {
        Message = "Recipe has non-public ingredients";
    }
}