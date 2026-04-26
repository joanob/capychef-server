using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Recipes.Domain.Cmd;
using Capychef.Recipes.Domain.DTO;

namespace Capychef.Recipes.Domain.Interfaces;

public interface IRecipeService
{
    Task<Result<RecipeDto>> CreateRecipe(AuthUserDetails userDetails, RecipeCmd cmd);

    Task<Result<RecipeDto>> UpdateRecipe(AuthUserDetails userDetails, int id, RecipeCmd cmd);

    Task<AppError?> DeleteRecipe(AuthUserDetails userDetails, int id);

    Task<Result<RecipeIngredientDto>> AddIngredient(AuthUserDetails userDetails, int recipeId, RecipeIngredientCmd cmd);

    Task<Result<RecipeIngredientDto>> UpdateIngredient(AuthUserDetails userDetails, int recipeId, int ingredientId,
        RecipeIngredientCmd cmd);

    Task<AppError?> DeleteIngredient(AuthUserDetails userDetails, int recipeId, int ingredientId);

    Task<Result<RecipeTagDto>> AddTag(AuthUserDetails userDetails, int recipeId, RecipeTagCmd cmd);

    Task<Result<RecipeTagDto>> UpdateTag(AuthUserDetails userDetails, int recipeId, int tagId, RecipeTagCmd cmd);

    Task<AppError?> DeleteTag(AuthUserDetails userDetails, int recipeId, int tagId);

    Task<Result<RecipeStepDto>> AddStep(AuthUserDetails userDetails, int recipeId, RecipeStepCmd cmd);

    Task<Result<RecipeStepDto>> UpdateStep(AuthUserDetails userDetails, int recipeId, int stepId, RecipeStepCmd cmd);

    Task<AppError?> DeleteStep(AuthUserDetails userDetails, int recipeId, int stepId);

    Task<Result<RecipeHouseholdDetailsDto>> CreateRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId,
        RecipeHouseholdDetailsCmd cmd);

    Task<Result<RecipeHouseholdDetailsDto>> CreateUserRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId,
        int userId, RecipeHouseholdDetailsCmd cmd);

    Task<Result<RecipeHouseholdDetailsDto>> UpdateRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId,
        int detailsId, RecipeHouseholdDetailsCmd cmd);

    Task<Result<RecipeHouseholdDetailsDto>> UpdateUserRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId,
        int userId, int detailsId, RecipeHouseholdDetailsCmd cmd);

    Task<AppError?> DeleteRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId, int detailsId);

    Task<AppError?> DeleteUserRecipeHouseholdDetails(AuthUserDetails userDetails, int recipeId, int userId,
        int detailsId);
}