using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Persistence;
using Capychef.Recipes.Domain.Cmd;
using Capychef.Recipes.Domain.DTO;
using Capychef.Recipes.Domain.Entities;
using Capychef.Recipes.Domain.Errors;
using Capychef.Recipes.Domain.Interfaces;

namespace Capychef.Recipes.Services;

public class RecipeService(CapychefDbContext dbContext, IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<Result<RecipeDto>> CreateRecipe(AuthUserDetails userDetails, RecipeCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeDto>(validationError);

        var recipe = new Recipe(cmd.Name, cmd.Description, cmd.Difficulty, cmd.CookingTimeMinutes, cmd.Servings,
            userDetails.GetHouseholdId(), userDetails.UserId);

        await recipeRepository.AddAsync(recipe);
        await dbContext.SaveChangesAsync();

        return new Result<RecipeDto>(new RecipeDto(recipe));
    }

    public async Task<Result<RecipeDto>> UpdateRecipe(AuthUserDetails userDetails, int id, RecipeCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeDto>(new NotFoundError(EntityType.Recipe, id));

        recipe.Name = cmd.Name;
        recipe.Description = cmd.Description;
        recipe.Difficulty = cmd.Difficulty;
        recipe.CookingTimeMinutes = cmd.CookingTimeMinutes;
        recipe.Servings = cmd.Servings;

        await dbContext.SaveChangesAsync();

        return new Result<RecipeDto>(new RecipeDto(recipe));
    }

    public async Task<AppError?> DeleteRecipe(AuthUserDetails userDetails, int id)
    {
        var recipe = await recipeRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (recipe == null) return new NotFoundError(EntityType.Recipe, id);

        recipe.Delete();
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<RecipeIngredientDto>> AddIngredient(AuthUserDetails userDetails, int recipeId,
        RecipeIngredientCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeIngredientDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeIngredientDto>(new NotFoundError(EntityType.Recipe, recipeId));

        if (cmd.AlternativeTo.HasValue)
        {
            var alternativeIngredient =
                await recipeRepository.FindTrackedIngredientById(cmd.AlternativeTo.Value, recipeId);
            if (alternativeIngredient == null)
                return new Result<RecipeIngredientDto>(new NotFoundError(EntityType.RecipeIngredient,
                    cmd.AlternativeTo.Value));
        }

        await recipeRepository.ShiftIngredientOrderNumFrom(recipeId, cmd.OrderNum);

        var ingredient = new RecipeIngredient(recipeId, cmd.OrderNum, cmd.FoodId, cmd.Quantity, cmd.FoodUoMId,
            cmd.AlternativeTo, userDetails.UserId);
        await recipeRepository.AddIngredientAsync(ingredient);

        await dbContext.SaveChangesAsync();

        return new Result<RecipeIngredientDto>(new RecipeIngredientDto(ingredient));
    }

    public async Task<Result<RecipeIngredientDto>> UpdateIngredient(AuthUserDetails userDetails, int recipeId,
        int ingredientId, RecipeIngredientCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeIngredientDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeIngredientDto>(new NotFoundError(EntityType.Recipe, recipeId));

        var ingredient = await recipeRepository.FindTrackedIngredientById(ingredientId, recipeId);
        if (ingredient == null)
            return new Result<RecipeIngredientDto>(new NotFoundError(EntityType.RecipeIngredient, ingredientId));

        if (cmd.AlternativeTo.HasValue)
        {
            var alternativeIngredient =
                await recipeRepository.FindTrackedIngredientById(cmd.AlternativeTo.Value, recipeId);
            if (alternativeIngredient == null)
                return new Result<RecipeIngredientDto>(new NotFoundError(EntityType.RecipeIngredient,
                    cmd.AlternativeTo.Value));
        }

        ingredient.FoodId = cmd.FoodId;
        ingredient.Quantity = cmd.Quantity;
        ingredient.FoodUoMId = cmd.FoodUoMId;
        ingredient.AlternativeTo = cmd.AlternativeTo;

        if (cmd.OrderNum != ingredient.OrderNum)
        {
            await recipeRepository.ShiftIngredientOrderNumFrom(recipeId, cmd.OrderNum);
            ingredient.OrderNum = cmd.OrderNum;
        }

        await dbContext.SaveChangesAsync();

        return new Result<RecipeIngredientDto>(new RecipeIngredientDto(ingredient));
    }

    public async Task<AppError?> DeleteIngredient(AuthUserDetails userDetails, int recipeId, int ingredientId)
    {
        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new NotFoundError(EntityType.Recipe, recipeId);

        var ingredient = await recipeRepository.FindTrackedIngredientById(ingredientId, recipeId);
        if (ingredient == null) return new NotFoundError(EntityType.RecipeIngredient, ingredientId);

        var alternatives = await recipeRepository.GetAlternativesTrackedByIngredientId(ingredientId);
        foreach (var alternative in alternatives) await recipeRepository.DeleteIngredientAsync(alternative);

        await recipeRepository.DeleteIngredientAsync(ingredient);
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<RecipeTagDto>> AddTag(AuthUserDetails userDetails, int recipeId, RecipeTagCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeTagDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeTagDto>(new NotFoundError(EntityType.Recipe, recipeId));

        await recipeRepository.ShiftTagOrderNumFrom(recipeId, cmd.OrderNum);

        var tag = new RecipeTag(recipeId, cmd.OrderNum, cmd.Tag, userDetails.UserId);
        await recipeRepository.AddTagAsync(tag);
        await dbContext.SaveChangesAsync();

        return new Result<RecipeTagDto>(new RecipeTagDto(tag));
    }

    public async Task<Result<RecipeTagDto>> UpdateTag(AuthUserDetails userDetails, int recipeId, int tagId,
        RecipeTagCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeTagDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeTagDto>(new NotFoundError(EntityType.Recipe, recipeId));

        var tag = await recipeRepository.FindTrackedTagById(tagId, recipeId);
        if (tag == null) return new Result<RecipeTagDto>(new NotFoundError(EntityType.RecipeTag, tagId));

        tag.Tag = cmd.Tag;

        if (cmd.OrderNum != tag.OrderNum)
        {
            await recipeRepository.ShiftTagOrderNumFrom(recipeId, cmd.OrderNum);
            tag.OrderNum = cmd.OrderNum;
        }

        await dbContext.SaveChangesAsync();

        return new Result<RecipeTagDto>(new RecipeTagDto(tag));
    }

    public async Task<AppError?> DeleteTag(AuthUserDetails userDetails, int recipeId, int tagId)
    {
        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new NotFoundError(EntityType.Recipe, recipeId);

        var tag = await recipeRepository.FindTrackedTagById(tagId, recipeId);
        if (tag == null) return new NotFoundError(EntityType.RecipeTag, tagId);

        await recipeRepository.DeleteTagAsync(tag);
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<RecipeStepDto>> AddStep(AuthUserDetails userDetails, int recipeId, RecipeStepCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeStepDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeStepDto>(new NotFoundError(EntityType.Recipe, recipeId));

        await recipeRepository.ShiftStepNumberFrom(recipeId, cmd.StepNumber);

        var step = new RecipeStep(recipeId, cmd.StepNumber, cmd.Description, userDetails.UserId);
        await recipeRepository.AddStepAsync(step);
        await dbContext.SaveChangesAsync();

        return new Result<RecipeStepDto>(new RecipeStepDto(step));
    }

    public async Task<Result<RecipeStepDto>> UpdateStep(AuthUserDetails userDetails, int recipeId, int stepId,
        RecipeStepCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<RecipeStepDto>(validationError);

        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new Result<RecipeStepDto>(new NotFoundError(EntityType.Recipe, recipeId));

        var step = await recipeRepository.FindTrackedStepById(stepId, recipeId);
        if (step == null) return new Result<RecipeStepDto>(new NotFoundError(EntityType.RecipeStep, stepId));

        step.Description = cmd.Description;

        if (cmd.StepNumber != step.StepNumber)
        {
            await recipeRepository.ShiftStepNumberFrom(recipeId, cmd.StepNumber);
            step.StepNumber = cmd.StepNumber;
        }

        await dbContext.SaveChangesAsync();

        return new Result<RecipeStepDto>(new RecipeStepDto(step));
    }

    public async Task<AppError?> DeleteStep(AuthUserDetails userDetails, int recipeId, int stepId)
    {
        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new NotFoundError(EntityType.Recipe, recipeId);

        var step = await recipeRepository.FindTrackedStepById(stepId, recipeId);
        if (step == null) return new NotFoundError(EntityType.RecipeStep, stepId);

        await recipeRepository.DeleteStepAsync(step);
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> PublishRecipe(AuthUserDetails userDetails, int recipeId)
    {
        var recipe = await recipeRepository.FindTrackedById(recipeId, userDetails.GetHouseholdId());
        if (recipe == null) return new NotFoundError(EntityType.Recipe, recipeId);

        var draft = Recipe.CreatePublicationDraft(recipe, userDetails.UserId);

        await recipeRepository.AddAsync(draft);

        var ingredients = await recipeRepository.GetIngredientsTrackedByRecipeId(recipeId);

        if (!ingredients.All(x => x.Food is { IsGlobal: true })) return new RecipeHasNonPublicIngredients();

        foreach (var ingredient in ingredients)
            await recipeRepository.AddIngredientAsync(
                new RecipeIngredient(draft, ingredient.OrderNum, ingredient.FoodId,
                    ingredient.Quantity, ingredient.FoodUoMId, null, userDetails.UserId));

        var tags = await recipeRepository.GetTagsTrackedByRecipeId(recipeId);
        foreach (var tag in tags)
            await recipeRepository.AddTagAsync(new RecipeTag(draft, tag.OrderNum, tag.Tag, userDetails.UserId));

        var steps = await recipeRepository.GetStepsTrackedByRecipeId(recipeId);
        foreach (var step in steps)
            await recipeRepository.AddStepAsync(new RecipeStep(draft, step.StepNumber, step.Description,
                userDetails.UserId));

        await dbContext.SaveChangesAsync();

        return null;
    }
}