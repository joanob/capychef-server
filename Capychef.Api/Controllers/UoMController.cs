using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("global-uom")]
public class UoMController(
    IServiceScopeFactory serviceScopeFactory
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> LoadGlobalUoM(GlobalUoMFileCmd fileCmd)
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