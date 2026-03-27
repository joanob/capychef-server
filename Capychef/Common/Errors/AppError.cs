using Capychef.Common.Entities;

namespace Capychef.Common.Errors;

public class AppError
{
    public AppError(ErrorType errorType)
    {
        ErrorType = errorType;
    }

    protected AppError(ErrorType errorType, EntityType entityType, int entityId)
    {
        ErrorType = errorType;
        Entity = new EntityDetails(entityType, entityId);
    }

    protected AppError(ErrorType errorType, EntityType entityType, string entityId)
    {
        ErrorType = errorType;
        Entity = new EntityDetails(entityType, entityId);
    }

    public ErrorType ErrorType { get; }
    public EntityDetails? Entity { get; }
    public string? Message { get; protected init; }

    public bool IsEqual(AppError error)
    {
        return ErrorType.Equals(error.ErrorType);
    }

    public override string ToString()
    {
        return ErrorType + ": " + Message;
    }
}