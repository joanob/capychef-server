using Capychef.Api.Authorization;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("storage-spaces")]
public class StorageSpacesController(
    IStorageSpaceService storageSpaceService
) : ControllerBase
{
    [CheckOwnership]
    [HttpPost]
    public async Task<ActionResult> CreateStorageSpace(CreateStorageSpaceCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var storageSpace = await storageSpaceService.CreateStorageSpace(cmd, userDetails);

        if (storageSpace.failed()) return GlobalErrorHandler.handleError(storageSpace.error());

        return Ok(storageSpace.get());
    }
}