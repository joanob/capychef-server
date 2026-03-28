using Capychef.Api.Errors;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("global-uom")]
public class UoMController(
    IUoMService uoMService
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UoMdto>>>> GetAllUoM()
    {
        var uoMs = await uoMService.GetAllUoM();

        return Ok(new ApiResponse<List<UoMdto>>(uoMs));
    }
}