namespace Capychef.Common.Entities;

public class EntityType
{
    private const string UserType = "USER";
    private const string UserPasswordType = "USER_PASSWORD";
    private const string TokenType = "TOKEN";

    private const string HouseholdType = "HOUSEHOLD";
    private const string HouseholdMemberType = "HOUSHOLD_MEMBER";
    private const string HouseholdInvitationType = "HOUSHOLD_INVITATION";
    private const string HouseholdJoinRequestType = "HOUSHOLD_JOIN_REQUEST";

    private const string StorageSpaceType = "STORAGE_SPACE";
    private const string StorageConditionType = "STORAGE_CONDITION";

    private const string UomType = "UOM";

    private const string FoodType = "FOOD";
    private const string FoodUoMType = "FOOD_UOM";
    private const string FoodBaseUoMType = "FOOD_BASE_UOM";
    private const string FoodCategoryType = "FOOD_CATEGORY";

    private const string BatchType = "BATCH";

    private const string SupermarketType = "SUPERMARKET";
    private const string SupermarketFoodDetailsType = "SUPERMARKET_FOOD_DETAILS";

    private readonly string _value;

    private EntityType(string value)
    {
        _value = value;
    }

    public static EntityType User => new(UserType);
    public static EntityType UserPassword => new(UserPasswordType);
    public static EntityType Token => new(TokenType);
    public static EntityType Household => new(HouseholdType);
    public static EntityType HouseholdMember => new(HouseholdMemberType);
    public static EntityType HouseholdInvitation => new(HouseholdInvitationType);
    public static EntityType HouseholdJoinRequest => new(HouseholdJoinRequestType);
    public static EntityType StorageSpace => new(StorageSpaceType);
    public static EntityType StorageCondition => new(StorageConditionType);
    public static EntityType UoM => new(UomType);
    public static EntityType Food => new(FoodType);
    public static EntityType FoodUoM => new(FoodUoMType);
    public static EntityType FoodBaseUoM => new(FoodBaseUoMType);
    public static EntityType FoodCategory => new(FoodCategoryType);
    public static EntityType Batch => new(BatchType);
    public static EntityType Supermarket => new(SupermarketType);
    public static EntityType SupermarketFoodDetails => new(SupermarketFoodDetailsType);

    public static EntityType From(string value)
    {
        return value switch
        {
            UserType => User,
            UserPasswordType => UserPassword,
            TokenType => Token,
            HouseholdType => Household,
            HouseholdMemberType => HouseholdMember,
            HouseholdInvitationType => HouseholdInvitation,
            HouseholdJoinRequestType => HouseholdJoinRequest,
            StorageSpaceType => StorageSpace,
            StorageConditionType => StorageCondition,
            UomType => UoM,
            FoodType => Food,
            FoodUoMType => FoodUoM,
            FoodBaseUoMType => FoodBaseUoM,
            FoodCategoryType => FoodCategory,
            BatchType => Batch,
            SupermarketType => Supermarket,
            SupermarketFoodDetailsType => SupermarketFoodDetails,
            _ => new EntityType("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}