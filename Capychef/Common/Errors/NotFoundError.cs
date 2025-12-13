using YourOwnBoss.Common.Entities;

namespace YourOwnBoss.Common.Errors;

public class NotFoundError : AppError
{
    private string _compositeId;
    private EntityType _entityType;
    private int _id;

    public NotFoundError(EntityType entityType, int id) : base(ErrorType.NOT_FOUND, "")
    {
        _entityType = entityType;
        _id = id;
        _message = entityType + " - " + id;
    }

    public NotFoundError(EntityType entityType, string compositeId) : base(ErrorType.NOT_FOUND, "")
    {
        _entityType = entityType;
        _compositeId = compositeId;
        _message = entityType + " - " + compositeId;
    }
}