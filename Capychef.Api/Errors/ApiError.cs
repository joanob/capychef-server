using Capychef.Common.Errors;

namespace Capychef.Api.Errors;

public class ApiError
{
    public ApiError(AppError error)
    {
        ErrorType = error.ErrorType.ToString();
        Entity = new ApiErrorEntityDetails(error.Entity);
        Message = error.Message;
    }

    public ApiError(AppError error, string errorCode)
    {
        ErrorType = error.ErrorType.ToString();
        ErrorCode = errorCode;
        Entity = new ApiErrorEntityDetails(error.Entity);
        Message = error.Message;
    }

    public string ErrorType { get; set; }
    public string? ErrorCode { get; set; }
    public ApiErrorEntityDetails? Entity { get; set; }
    public string? Message { get; set; }
}

public class ApiErrorEntityDetails
{
    public ApiErrorEntityDetails(EntityDetails? entity)
    {
        EntityType = entity.EntityType.ToString();
        EntityId = entity.EntityId;
        EntityIdString = entity.EntityIdString;
    }

    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public string? EntityIdString { get; set; }
}