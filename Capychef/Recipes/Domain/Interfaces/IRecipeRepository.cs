using Capychef.Recipes.Domain.Entities;

namespace Capychef.Recipes.Domain.Interfaces;

public interface IRecipeRepository
{
    Task AddAsync(Recipe recipe);

    Task<Recipe?> FindTrackedById(int id, int householdId);

    Task<List<RecipeIngredient>> GetIngredientsTrackedByRecipeId(int recipeId);

    Task AddIngredientAsync(RecipeIngredient ingredient);

    Task<RecipeIngredient?> FindTrackedIngredientById(int id, int recipeId);

    Task DeleteIngredientAsync(RecipeIngredient ingredient);

    Task<List<RecipeTag>> GetTagsTrackedByRecipeId(int recipeId);

    Task AddTagAsync(RecipeTag tag);

    Task<RecipeTag?> FindTrackedTagById(int id, int recipeId);

    Task DeleteTagAsync(RecipeTag tag);

    Task<List<RecipeStep>> GetStepsTrackedByRecipeId(int recipeId);

    Task AddStepAsync(RecipeStep step);

    Task<RecipeStep?> FindTrackedStepById(int id, int recipeId);

    Task DeleteStepAsync(RecipeStep step);

    Task<List<RecipeIngredient>> GetAlternativesTrackedByIngredientId(int ingredientId);

    Task<int> GetNextIngredientOrderNum(int recipeId);

    Task<int> GetNextTagOrderNum(int recipeId);

    Task<int> GetNextStepNumber(int recipeId);

    Task ShiftIngredientOrderNumFrom(int recipeId, int fromOrderNum);

    Task ShiftTagOrderNumFrom(int recipeId, int fromOrderNum);

    Task ShiftStepNumberFrom(int recipeId, int fromStepNumber);
}