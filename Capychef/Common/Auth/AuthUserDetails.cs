namespace Capychef.Common.Auth;

public class AuthUserDetails
{
    private int? _householdId;

    public AuthUserDetails()
    {
    }

    public AuthUserDetails(int userId, int sessionId)
    {
        UserId = userId;
        SessionId = sessionId;
    }

    public AuthUserDetails(int userId, int sessionId, int householdId)
    {
        UserId = userId;
        SessionId = sessionId;
        _householdId = householdId;
    }

    public int UserId { get; }
    public int SessionId { get; }

    public bool HasHouseholdId => _householdId.HasValue;

    public bool IsValid()
    {
        return UserId != 0;
    }

    public int GetHouseholdId()
    {
        if (!_householdId.HasValue)
            throw new Exception($"HouseholdId is not set for user {UserId} session {SessionId}");

        return _householdId.Value;
    }

    public void SetHousehold(int householdId)
    {
        _householdId = householdId;
    }

    public void DeselectHousehold()
    {
        _householdId = null;
    }
}