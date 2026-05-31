using Capychef.Common.Auth;

namespace Capychef.Api.Realtime;

public class ConnectionDetails
{
    public ConnectionDetails(string connectionId, AuthUserDetails userDetails)
    {
        ConnectionId = connectionId;
        UserId = userDetails.UserId;
        SessionId = userDetails.SessionId;
        HouseholdId = userDetails.HasHouseholdId ? userDetails.GetHouseholdId() : null;
    }

    public string ConnectionId { get; private set; }
    public int UserId { get; private set; }
    public int SessionId { get; private set; }
    public int? HouseholdId { get; private set; }
    public DateTime ConnectedAt { get; init; } = DateTime.UtcNow;

    public void UpdateHouseholdId(int? householdId)
    {
        HouseholdId = householdId;
    }
}