namespace Capychef.Common.Errors;

public class ErrorType
{
    private const string NotFoundConst = "NOT_FOUND";
    private const string AuthenticationConst = "AUTHENTICATION";
    private const string AuthorizationConst = "AUTHORIZATION";
    private const string ValidationConst = "VALIDATION";
    private const string CannotCreateConst = "CANNOT_CREATE";
    private const string SubscriptionRequiredConst = "SUBSCRIPTION_REQUIRED";
    private const string ConflictConst = "CONFLICT";
    private const string UnknownConst = "UNKNOWN_ERROR";

    private readonly string _value;

    private ErrorType(string value)
    {
        _value = value;
    }

    public static ErrorType NotFound => new(NotFoundConst);
    public static ErrorType Authentication => new(AuthenticationConst);
    public static ErrorType Authorization => new(AuthorizationConst);
    public static ErrorType Validation => new(ValidationConst);
    public static ErrorType CannotCreate => new(CannotCreateConst);
    public static ErrorType SubscriptionRequired => new(SubscriptionRequiredConst);
    public static ErrorType Conflict => new(ConflictConst);
    public static ErrorType Unknown => new(UnknownConst);

    public override string ToString()
    {
        return _value;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            ErrorType error => error.ToString().Equals(_value),
            _ => false
        };
    }

    protected bool Equals(ErrorType other)
    {
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}