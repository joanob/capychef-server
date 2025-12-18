namespace Capychef.Common.Auth;

public class AuthUserDetails
{
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
    public int? HouseholdId { get; }
}