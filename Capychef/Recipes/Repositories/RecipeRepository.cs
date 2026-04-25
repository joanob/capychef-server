using Capychef.Persistence;
using Capychef.Recipes.Domain.Entities;
using Capychef.Recipes.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Recipes.Repositories;

public class RecipeRepository(CapychefDbContext dbContext) : IRecipeRepository
{
    public async Task AddAsync(Recipe recipe)
    {
        await dbContext.Recipes.AddAsync(recipe);
    }

    public async Task<Recipe?> FindTrackedById(int id, int householdId)
    {
        return await dbContext.Recipes
            .Where(x => !x.IsDeleted && x.HouseholdId == householdId && x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Recipe?> FindTrackedPublicByPrivateRecipeId(int privateRecipeId)
    {
        return await dbContext.Recipes
            .Where(x => !x.IsDeleted && x.Type == RecipeType.Public && x.HouseholdRecipeId == privateRecipeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Recipe?> FindTrackedPendingDraftByPrivateRecipeId(int privateRecipeId)
    {
        return await dbContext.Recipes
            .Where(x => !x.IsDeleted && x.Type != RecipeType.Public && x.ReviewedAt == null &&
                        x.HouseholdRecipeId == privateRecipeId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RecipeIngredient>> GetIngredientsTrackedByRecipeId(int recipeId)
    {
        return await dbContext.RecipeIngredients.Include(x => x.Food)
            .Where(x => x.RecipeId == recipeId)
            .OrderBy(x => x.OrderNum)
            .ToListAsync();
    }

    public async Task AddIngredientAsync(RecipeIngredient ingredient)
    {
        await dbContext.RecipeIngredients.AddAsync(ingredient);
    }

    public async Task<RecipeIngredient?> FindTrackedIngredientById(int id, int recipeId)
    {
        return await dbContext.RecipeIngredients
            .Where(x => x.Id == id && x.RecipeId == recipeId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RecipeIngredient>> GetAlternativesTrackedByIngredientId(int ingredientId)
    {
        return await dbContext.RecipeIngredients
            .Where(x => x.AlternativeTo == ingredientId)
            .ToListAsync();
    }

    public async Task DeleteIngredientAsync(RecipeIngredient ingredient)
    {
        dbContext.RecipeIngredients.Remove(ingredient);
        await Task.CompletedTask;
    }

    public async Task<List<RecipeTag>> GetTagsTrackedByRecipeId(int recipeId)
    {
        return await dbContext.RecipeTags
            .Where(x => x.RecipeId == recipeId)
            .OrderBy(x => x.OrderNum)
            .ToListAsync();
    }

    public async Task AddTagAsync(RecipeTag tag)
    {
        await dbContext.RecipeTags.AddAsync(tag);
    }

    public async Task<RecipeTag?> FindTrackedTagById(int id, int recipeId)
    {
        return await dbContext.RecipeTags
            .Where(x => x.Id == id && x.RecipeId == recipeId)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteTagAsync(RecipeTag tag)
    {
        dbContext.RecipeTags.Remove(tag);
        await Task.CompletedTask;
    }

    public async Task<List<RecipeStep>> GetStepsTrackedByRecipeId(int recipeId)
    {
        return await dbContext.RecipeSteps
            .Where(x => x.RecipeId == recipeId)
            .OrderBy(x => x.StepNumber)
            .ToListAsync();
    }

    public async Task AddStepAsync(RecipeStep step)
    {
        await dbContext.RecipeSteps.AddAsync(step);
    }

    public async Task<RecipeStep?> FindTrackedStepById(int id, int recipeId)
    {
        return await dbContext.RecipeSteps
            .Where(x => x.Id == id && x.RecipeId == recipeId)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteStepAsync(RecipeStep step)
    {
        dbContext.RecipeSteps.Remove(step);
        await Task.CompletedTask;
    }

    public async Task<int> GetNextIngredientOrderNum(int recipeId)
    {
        var max = await dbContext.RecipeIngredients
            .Where(x => x.RecipeId == recipeId)
            .Select(x => (int?)x.OrderNum)
            .MaxAsync();
        return (max ?? 0) + 1;
    }

    public async Task<int> GetNextTagOrderNum(int recipeId)
    {
        var max = await dbContext.RecipeTags
            .Where(x => x.RecipeId == recipeId)
            .Select(x => (int?)x.OrderNum)
            .MaxAsync();
        return (max ?? 0) + 1;
    }

    public async Task<int> GetNextStepNumber(int recipeId)
    {
        var max = await dbContext.RecipeSteps
            .Where(x => x.RecipeId == recipeId)
            .Select(x => (int?)x.StepNumber)
            .MaxAsync();
        return (max ?? 0) + 1;
    }

    public async Task ShiftIngredientOrderNumFrom(int recipeId, int fromOrderNum)
    {
        var ingredients = await dbContext.RecipeIngredients
            .Where(x => x.RecipeId == recipeId && x.OrderNum >= fromOrderNum)
            .ToListAsync();

        foreach (var ingredient in ingredients)
            ingredient.OrderNum++;
    }

    public async Task ShiftTagOrderNumFrom(int recipeId, int fromOrderNum)
    {
        var tags = await dbContext.RecipeTags
            .Where(x => x.RecipeId == recipeId && x.OrderNum >= fromOrderNum)
            .ToListAsync();

        foreach (var tag in tags)
            tag.OrderNum++;
    }

    public async Task ShiftStepNumberFrom(int recipeId, int fromStepNumber)
    {
        var steps = await dbContext.RecipeSteps
            .Where(x => x.RecipeId == recipeId && x.StepNumber >= fromStepNumber)
            .ToListAsync();

        foreach (var step in steps)
            step.StepNumber++;
    }
}