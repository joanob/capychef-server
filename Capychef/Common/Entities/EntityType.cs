namespace YourOwnBoss.Common.Entities;

public class EntityType
{
    private const string _userType = "USER";
    private const string _tokenType = "TOKEN";
    private readonly string _value;

    private EntityType(string value)
    {
        _value = value;
    }

    public static EntityType User => new(_userType);
    public static EntityType Token => new(_tokenType);

    public static EntityType from(string value)
    {
        return value switch
        {
            _userType => User,
            _ => null
        };
    }

    public override string ToString()
    {
        return _value;
    }
}