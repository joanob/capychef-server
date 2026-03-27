using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthCheckController(HealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> CheckServerHealth()
    {
        var status = await healthCheckService.CheckServiceHealth();

        return Ok(status);
    }
}