namespace Capychef.Common.Errors;

public class AppError
{
    protected string _message;

    public AppError(ErrorType type, string message)
    {
        errorType = type;
        _message = message;
    }

    public ErrorType errorType { get; protected set; }

    public bool isEqual(AppError error)
    {
        return errorType == error.errorType;
    }

    public override string ToString()
    {
        return errorType + ": " + _message;
    }
}