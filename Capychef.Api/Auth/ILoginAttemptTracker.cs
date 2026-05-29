namespace Capychef.Api.Auth;

public interface ILoginAttemptTracker
{
    Task<bool> IsBlockedAsync(string ip);
    Task RecordFailedAttemptAsync(string ip);
    Task ResetAsync(string ip);
}
