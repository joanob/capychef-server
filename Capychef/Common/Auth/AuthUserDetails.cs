namespace Capychef.Common.Auth;

public class AuthUserDetails(int userId, int sessionId)
{
    public int UserId { get; } = userId;
    public int SessionId { get; } = sessionId;
}