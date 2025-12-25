using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("testdata")]
public class TestdataController(Testdata.Testdata testdata) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GenerateTestdata()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development") return NotFound();

        testdata.Generate();

        return Ok();
    }
}