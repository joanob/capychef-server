using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;
using Capychef.Storage.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("storage")]
public class StorageController(
    IBatchService batchService,
    ILoggerFactory loggerFactory) : ControllerBase
{
    [CheckMembership]
    [HttpPost]
    public async Task<ActionResult<BatchDTO>> CreateBatch(CreateBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.CreateBatch(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("BatchService.CreateBatch");

        if (batch.failed()) return GlobalErrorHandler.handleError(batch.error(), logger);

        return Ok(batch.get());
    }

    [CheckMembership]
    [HttpGet]
    public async Task<ActionResult<List<BatchDTO>>> GetAllBatches()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batches = await batchService.GetAllBatches(userDetails);

        return Ok(batches);
    }

    [CheckMembership]
    [HttpPut("{id}/consume")]
    public async Task<ActionResult<BatchDTO>> ConsumeBatch(int id, ConsumeBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.ConsumeBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.ConsumeBatch");

        if (batch.failed()) return GlobalErrorHandler.handleError(batch.error(), logger);

        return Ok(batch.get());
    }

    [CheckMembership]
    [HttpPut("{id}/discard")]
    public async Task<ActionResult<BatchDTO>> DiscardBatch(int id, DiscardBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.DiscardBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.DiscardBatch");

        if (batch.failed()) return GlobalErrorHandler.handleError(batch.error(), logger);

        return Ok(batch.get());
    }
}