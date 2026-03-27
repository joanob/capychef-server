using Capychef.Api.Errors;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("global-uom")]
public class UoMController(
    IServiceScopeFactory serviceScopeFactory,
    IUoMService uoMService
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UoMdto>>>> GetAllUoM()
    {
        var uoMs = await uoMService.GetAllUoM();

        return Ok(new ApiResponse<List<UoMdto>>(uoMs));
    }


    [HttpPost]
    public ActionResult LoadGlobalUoM(GlobalUoMFileCmd fileCmd)
    {
        Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            var globalUoMService = scope.ServiceProvider.GetRequiredService<IUoMService>();

            try
            {
                await globalUoMService.LoadGlobalUoM(fileCmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });

        return Ok();
    }
}