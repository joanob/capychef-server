namespace Capychef.Common.Errors;

public class ErrorType
{
    private const string _notFound = "NOT_FOUND";
    private const string _authentication = "AUTHENTICATION";
    private const string _authorization = "AUTHORIZATION";
    private const string _validation = "VALIDATION";
    private const string _cannotCreate = "CANNOT_CREATE";
    private const string _subscriptionRequired = "SUBSCRIPTION_REQUIRED";
    private const string _unknown = "UNKNOWN_ERROR";

    private readonly string _value;

    private ErrorType(string value)
    {
        _value = value;
    }

    public static ErrorType NotFound => new(_notFound);
    public static ErrorType Authentication => new(_authentication);
    public static ErrorType Authorization => new(_authorization);
    public static ErrorType Validation => new(_validation);
    public static ErrorType CannotCreate => new(_cannotCreate);
    public static ErrorType SubscriptionRequired => new(_subscriptionRequired);
    public static ErrorType Unknown => new(_unknown);

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
}