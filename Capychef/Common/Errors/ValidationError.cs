namespace Capychef.Common.Errors;

public class ValidationError : AppError
{
    public ValidationError(string message) : base(ErrorType.Validation)
    {
        Message = message;
    }

    public ValidationError(string affectedClass, string field, string message) : base(ErrorType.Validation)
    {
        Message = affectedClass + " " + field + " " + message;
    }
}