using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("testdata")]
public class TestdataController(IServiceScopeFactory serviceScopeFactory) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GenerateTestdata()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development") return NotFound();

        Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            var testdata = scope.ServiceProvider.GetRequiredService<Testdata.Testdata>();

            await testdata.Generate();
        });

        return Ok();
    }
}