using Capychef.Common.Entities;

namespace Capychef.Common.Errors;

public class AppError
{
    public AppError(ErrorType errorType)
    {
        ErrorType = errorType;
    }

    public AppError(ErrorType errorType, EntityType entityType)
    {
        ErrorType = errorType;
        Entity = new EntityDetails(entityType);
    }

    public AppError(ErrorType errorType, EntityType entityType, int entityId)
    {
        ErrorType = errorType;
        Entity = new EntityDetails(entityType, entityId);
    }

    public AppError(ErrorType errorType, EntityType entityType, string entityId)
    {
        ErrorType = errorType;
        Entity = new EntityDetails(entityType, entityId);
    }

    public ErrorType ErrorType { get; protected set; }
    public EntityDetails? Entity { get; protected set; }
    public string? Message { get; protected set; }

    public bool isEqual(AppError error)
    {
        return ErrorType == error.ErrorType;
    }

    public override string ToString()
    {
        return ErrorType + ": " + Message;
    }
}