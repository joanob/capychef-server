using Capychef.Common.Auth;
using Capychef.Common.Result;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchService
{
    Task<Result<BatchDTO>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd);
    Task<List<BatchDTO>> GetAllBatches(AuthUserDetails userDetails);
    Task<Result<BatchDTO>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<BatchDTO>> ConsumeBatch(int batchId, ConsumeBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<BatchDTO>> DiscardBatch(int batchId, DiscardBatchCmd cmd, AuthUserDetails userDetails);
    Task<Result<List<BatchDTO>>> MoveBatch(int batchId, MoveBatchCmd cmd, AuthUserDetails userDetails);
}