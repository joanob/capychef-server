using Capychef.DataLoader.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("data")]
public class DataLoaderController(
    IServiceScopeFactory serviceScopeFactory
) : ControllerBase
{
    [HttpPost("load")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> LoadData(IFormFile? file)
    {
        if (file == null || file.Length == 0) return BadRequest("Se requiere un fichero JSON");

        using var scope = serviceScopeFactory.CreateScope();

        var loader = scope.ServiceProvider.GetRequiredService<IDataLoader>();

        try
        {
            using var stream = file.OpenReadStream();
            await loader.LoadFromStream(stream);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Error al procesar el fichero");
        }

        return Ok();
    }
}