using Capychef.Common.Auth;
using Capychef.Common.Result;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchService
{
    Task<Result<BatchDto>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd);

    Task<Result<BatchModificationDto>> MoveBatch(int batchId, MoveBatchCmd cmd, AuthUserDetails userDetails);

    Task<Result<BatchModificationDto>> ConsumeBatch(int batchId, ModifyBatchCmd cmd, AuthUserDetails userDetails);

    Task<Result<BatchModificationDto>> DiscardBatch(int batchId, ModifyBatchCmd cmd, AuthUserDetails userDetails);

    Task<Result<BatchDto>> UpdateBatch(int batchId, UpdateBatchCmd cmd, AuthUserDetails userDetails);

    Task<List<BatchDto>> GetAllBatches(AuthUserDetails userDetails);
}