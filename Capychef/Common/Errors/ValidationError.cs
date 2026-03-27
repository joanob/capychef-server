namespace Capychef.Common.Errors;

public class ValidationError : AppError
{
    public ValidationError(string message) : base(ErrorType.Validation)
    {
        Message = message;
    }
}