namespace Capychef.Common.Auth;

public class AuthUserDetails
{
    private int? HouseholdId;

    public AuthUserDetails(int userId, int sessionId)
    {
        UserId = userId;
        SessionId = sessionId;
    }

    public AuthUserDetails(int userId, int sessionId, int householdId)
    {
        UserId = userId;
        SessionId = sessionId;
        HouseholdId = householdId;
    }

    public int UserId { get; }
    public int SessionId { get; }

    public bool HasHouseholdId => HouseholdId.HasValue;

    public int GetHouseholdId()
    {
        if (!HouseholdId.HasValue) throw new Exception($"HouseholdId is not set for user {UserId} session {SessionId}");

        return HouseholdId.Value;
    }

    public void setHousehold(int householdId)
    {
        HouseholdId = householdId;
    }
}