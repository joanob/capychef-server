namespace Capychef.Common.Errors;

public class ConcurrencyError : AppError
{
    public ConcurrencyError() : base(ErrorType.Conflict)
    {
        Message = "The resource was modified by another request. Please reload and try again.";
    }
}