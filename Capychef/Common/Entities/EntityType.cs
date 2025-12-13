namespace YourOwnBoss.Common.Entities;

public class EntityType
{
    private readonly string _value;
    
    private const string _userType = "USER";

    public static EntityType User => new(_userType);
    
    public static EntityType from(string value)
    {
        return value switch
        {
            _userType => User,
            _ => null
        };
    }
    
    private EntityType(string value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }
}