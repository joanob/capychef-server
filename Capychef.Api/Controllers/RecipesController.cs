using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Recipes.Domain.Cmd;
using Capychef.Recipes.Domain.DTO;
using Capychef.Recipes.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("recipes")]
public class RecipesController(IRecipeService recipeService, ILoggerFactory loggerFactory) : ControllerBase
{
    [CheckMembership]
    [HttpPost]
    public async Task<ActionResult<RecipeDto>> CreateRecipe(RecipeCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.CreateRecipe(userDetails, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.CreateRecipe");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, RecipeCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.UpdateRecipe(userDetails, id, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateRecipe");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteRecipe(userDetails, id);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteRecipe");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }

    [CheckMembership]
    [HttpPost("{recipeId}/ingredients")]
    public async Task<ActionResult<RecipeIngredientDto>> AddIngredient(int recipeId, RecipeIngredientCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.AddIngredient(userDetails, recipeId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.AddIngredient");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{recipeId}/ingredients/{ingredientId}")]
    public async Task<ActionResult<RecipeIngredientDto>> UpdateIngredient(int recipeId, int ingredientId,
        RecipeIngredientCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.UpdateIngredient(userDetails, recipeId, ingredientId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateIngredient");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("{recipeId}/ingredients/{ingredientId}")]
    public async Task<ActionResult> DeleteIngredient(int recipeId, int ingredientId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteIngredient(userDetails, recipeId, ingredientId);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteIngredient");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }

    [CheckMembership]
    [HttpPost("{recipeId}/tags")]
    public async Task<ActionResult<RecipeTagDto>> AddTag(int recipeId, RecipeTagCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.AddTag(userDetails, recipeId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.AddTag");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{recipeId}/tags/{tagId}")]
    public async Task<ActionResult<RecipeTagDto>> UpdateTag(int recipeId, int tagId, RecipeTagCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.UpdateTag(userDetails, recipeId, tagId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateTag");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("{recipeId}/tags/{tagId}")]
    public async Task<ActionResult> DeleteTag(int recipeId, int tagId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteTag(userDetails, recipeId, tagId);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteTag");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }

    [CheckMembership]
    [HttpPost("{recipeId}/steps")]
    public async Task<ActionResult<RecipeStepDto>> AddStep(int recipeId, RecipeStepCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.AddStep(userDetails, recipeId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.AddStep");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{recipeId}/steps/{stepId}")]
    public async Task<ActionResult<RecipeStepDto>> UpdateStep(int recipeId, int stepId, RecipeStepCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.UpdateStep(userDetails, recipeId, stepId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateStep");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("{recipeId}/steps/{stepId}")]
    public async Task<ActionResult> DeleteStep(int recipeId, int stepId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteStep(userDetails, recipeId, stepId);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteStep");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }

    [CheckMembership]
    [HttpPost("{recipeId}/household-details")]
    public async Task<ActionResult<RecipeHouseholdDetailsDto>> CreateHouseholdDetails(int recipeId,
        RecipeHouseholdDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.CreateRecipeHouseholdDetails(userDetails, recipeId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.CreateHouseholdDetails");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPost("{recipeId}/users/{userId}/household-details")]
    public async Task<ActionResult<RecipeHouseholdDetailsDto>> CreateUserHouseholdDetails(int recipeId, int userId,
        RecipeHouseholdDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.CreateUserRecipeHouseholdDetails(userDetails, recipeId, userId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.CreateUserHouseholdDetails");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{recipeId}/household-details/{detailsId}")]
    public async Task<ActionResult<RecipeHouseholdDetailsDto>> UpdateHouseholdDetails(int recipeId, int detailsId,
        RecipeHouseholdDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result = await recipeService.UpdateRecipeHouseholdDetails(userDetails, recipeId, detailsId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateHouseholdDetails");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("{recipeId}/users/{userId}/household-details/{detailsId}")]
    public async Task<ActionResult<RecipeHouseholdDetailsDto>> UpdateUserHouseholdDetails(int recipeId, int userId,
        int detailsId, RecipeHouseholdDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var result =
            await recipeService.UpdateUserRecipeHouseholdDetails(userDetails, recipeId, userId, detailsId, cmd);
        var logger = loggerFactory.CreateLogger("RecipeService.UpdateUserHouseholdDetails");
        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);
        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("{recipeId}/household-details/{detailsId}")]
    public async Task<ActionResult> DeleteHouseholdDetails(int recipeId, int detailsId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteRecipeHouseholdDetails(userDetails, recipeId, detailsId);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteHouseholdDetails");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }

    [CheckMembership]
    [HttpDelete("{recipeId}/users/{userId}/household-details/{detailsId}")]
    public async Task<ActionResult> DeleteUserHouseholdDetails(int recipeId, int userId, int detailsId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);
        var error = await recipeService.DeleteUserRecipeHouseholdDetails(userDetails, recipeId, userId, detailsId);
        var logger = loggerFactory.CreateLogger("RecipeService.DeleteUserHouseholdDetails");
        if (error != null) return GlobalErrorHandler.HandleError(error, logger);
        return Ok();
    }
}