using Capychef.Common.Entities;

namespace Capychef.Common.Errors;

public class NotFoundError : AppError
{
    public NotFoundError(EntityType entityType, int id) : base(ErrorType.NotFound, entityType, id)
    {
    }

    public NotFoundError(EntityType entityType, string isString) : base(ErrorType.NotFound, entityType, isString)
    {
    }
}