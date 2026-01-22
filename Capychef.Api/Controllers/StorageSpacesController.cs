using Capychef.Api.Authorization;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("storage-spaces")]
public class StorageSpacesController(
    IStorageSpaceService storageSpaceService,
    ILoggerFactory loggerFactory
) : ControllerBase
{
    [CheckOwnership]
    [HttpPost]
    public async Task<ActionResult<StorageSpaceDTO>> CreateStorageSpace(CreateStorageSpaceCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var storageSpace = await storageSpaceService.CreateStorageSpace(cmd, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.CreateStorageSpace");

        if (storageSpace.failed()) return GlobalErrorHandler.handleError(storageSpace.error(), logger);

        return Ok(storageSpace.get());
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<StorageSpaceDTO>> UpdateStorageSpace(int id, UpdateStorageSpaceCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var storageSpace = await storageSpaceService.UpdateStorageSpace(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.UpdateStorageSpace");

        if (storageSpace.failed()) return GlobalErrorHandler.handleError(storageSpace.error(), logger);

        return Ok(storageSpace.get());
    }

    [CheckOwnership]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStorageSpace(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await storageSpaceService.DeleteStorageSpace(id, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.DeleteStorageSpace");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        return Ok();
    }
}