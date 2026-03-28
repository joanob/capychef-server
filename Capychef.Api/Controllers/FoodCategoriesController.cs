using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food-categories")]
public class FoodCategoriesController(
    IFoodCategoryService foodCategoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FoodCategoryDto>>> GetAllFoodCategories([FromQuery] string? displayMode)
    {
        if (displayMode == "tree") return Ok(await foodCategoryService.GetAllCategoriesAsTree());

        return Ok(await foodCategoryService.GetAllCategories());
    }
}