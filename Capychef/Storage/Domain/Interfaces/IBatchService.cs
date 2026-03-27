using Capychef.Common.Auth;
using Capychef.Common.Result;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchService
{
    Task<Result<BatchDto>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd);
    Task<List<BatchDto>> GetAllBatches(AuthUserDetails userDetails);
    Task<Result<BatchDto>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<BatchDto>> ConsumeBatch(int batchId, ConsumeBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<BatchDto>> DiscardBatch(int batchId, DiscardBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<List<BatchDto>>> MoveBatch(int batchId, MoveBatchCmd cmd, AuthUserDetails userDetails);
}