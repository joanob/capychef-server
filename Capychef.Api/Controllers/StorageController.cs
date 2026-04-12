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
    public async Task<ActionResult<BatchDto>> CreateBatch(CreateBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.CreateBatch(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("BatchService.CreateBatch");

        if (batch.Failed()) return GlobalErrorHandler.HandleError(batch.Error(), logger);

        return Ok(batch.Get());
    }

    [CheckMembership]
    [HttpPut("{id}/move")]
    public async Task<ActionResult<BatchDto>> MoveBatch(int id, MoveBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.MoveBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.MoveBatch");

        if (batch.Failed()) return GlobalErrorHandler.HandleError(batch.Error(), logger);

        return Ok(batch.Get());
    }

    [CheckMembership]
    [HttpPut("{id}/consume")]
    public async Task<ActionResult<BatchDto>> ConsumeBatch(int id, ModifyBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.ConsumeBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.ConsumeBatch");

        if (batch.Failed()) return GlobalErrorHandler.HandleError(batch.Error(), logger);

        return Ok(batch.Get());
    }

    [CheckMembership]
    [HttpPut("{id}/discard")]
    public async Task<ActionResult<BatchDto>> DiscardBatch(int id, ModifyBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.DiscardBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.DiscardBatch");

        if (batch.Failed()) return GlobalErrorHandler.HandleError(batch.Error(), logger);

        return Ok(batch.Get());
    }

    [CheckMembership]
    [HttpGet]
    public async Task<ActionResult<List<BatchDto>>> GetAllBatches()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batches = await batchService.GetAllBatches(userDetails);

        return Ok(batches);
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<BatchDto>> UpdateBatch(int id, UpdateBatchCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var batch = await batchService.UpdateBatch(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("BatchService.UpdateBatch");

        if (batch.Failed()) return GlobalErrorHandler.HandleError(batch.Error(), logger);

        return Ok(batch.Get());
    }
}