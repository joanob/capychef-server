namespace YourOwnBoss.Common.Entities;

public class EntityType
{
    private const string _userType = "USER";
    private const string _userPasswordType = "USER_PASSWORD";
    private const string _tokenType = "TOKEN";

    private const string _householdType = "HOUSHOLD";
    private const string _householdInvitationType = "HOUSHOLD_INVITATION";
    private const string _householdJoinRequestType = "HOUSHOLD_JOIN_REQUEST";

    private const string _storageConditionType = "STORAGE_CONDITION";

    private const string _uomType = "UOM";

    private const string _foodType = "FOOD";
    private const string _foodCategoryType = "FOOD_CATEGORY";

    private readonly string _value;

    private EntityType(string value)
    {
        _value = value;
    }

    public static EntityType User => new(_userType);
    public static EntityType UserPassword => new(_userPasswordType);
    public static EntityType Token => new(_tokenType);
    public static EntityType Household => new(_householdType);
    public static EntityType HouseholdInvitation => new(_householdInvitationType);
    public static EntityType HouseholdJoinRequest => new(_householdJoinRequestType);
    public static EntityType StorageCondition => new(_storageConditionType);
    public static EntityType UoM => new(_uomType);
    public static EntityType Food => new(_foodType);
    public static EntityType FoodCategory => new(_foodCategoryType);

    public static EntityType from(string value)
    {
        return value switch
        {
            _userType => User,
            _userPasswordType => UserPassword,
            _tokenType => Token,
            _householdType => Household,
            _householdInvitationType => HouseholdInvitation,
            _householdJoinRequestType => HouseholdJoinRequest,
            _storageConditionType => StorageCondition,
            _uomType => UoM,
            _foodType => Food,
            _foodCategoryType => FoodCategory,
            _ => null
        };
    }

    public override string ToString()
    {
        return _value;
    }
}