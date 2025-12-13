using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthCheckController(HealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<String>> CheckServerHealth()
    {
        var status = await healthCheckService.checkServiceHealth();
        
        return Ok(status);
    }
}