using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<StorageSpaceDto>> CreateStorageSpace(StorageSpaceCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var storageSpace = await storageSpaceService.CreateStorageSpace(cmd, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.CreateStorageSpace");

        if (storageSpace.Failed()) return GlobalErrorHandler.HandleError(storageSpace.Error(), logger);

        return Ok(storageSpace.Get());
    }

    [CheckMembership]
    [HttpGet("household/{householdId}")]
    public async Task<ActionResult<ApiResponse<List<StorageSpaceDto>>>> GetHouseholdStorageSpaces(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        if (householdId != userDetails.GetHouseholdId())
            return BadRequest(new ApiResponse<List<StorageSpaceDto>>(
                new ApiError(new ValidationError("HouseholdId does not match the active household"))));

        var storageSpaces = await storageSpaceService.GetHouseholdStorageSpaces(userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.GetHouseholdStorageSpaces");

        if (storageSpaces.Failed()) return GlobalErrorHandler.HandleError(storageSpaces.Error(), logger);

        return Ok(new ApiResponse<List<StorageSpaceDto>>(storageSpaces.Get()));
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<StorageSpaceDto>> UpdateStorageSpace(int id, StorageSpaceCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var storageSpace = await storageSpaceService.UpdateStorageSpace(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.UpdateStorageSpace");

        if (storageSpace.Failed()) return GlobalErrorHandler.HandleError(storageSpace.Error(), logger);

        return Ok(storageSpace.Get());
    }

    [CheckOwnership]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStorageSpace(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await storageSpaceService.DeleteStorageSpace(id, userDetails);

        var logger = loggerFactory.CreateLogger("StorageSpaceService.DeleteStorageSpace");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [HttpGet("initial")]
    public async Task<ActionResult<ApiResponse<List<InitialStorageSpaceDto>>>> GetInitialStorageSpaces()
    {
        var result = await storageSpaceService.GetInitialStorageSpaces();

        var logger = loggerFactory.CreateLogger("StorageSpaceService.GetInitialStorageSpaces");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        return Ok(new ApiResponse<List<InitialStorageSpaceDto>>(result.Get()));
    }
}