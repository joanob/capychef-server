namespace Capychef.Api.Auth;

public class NoOpLoginAttemptTracker : ILoginAttemptTracker
{
    public Task<bool> IsBlockedAsync(string ip)
    {
        return Task.FromResult(false);
    }

    public Task RecordFailedAttemptAsync(string ip)
    {
        return Task.CompletedTask;
    }

    public Task ResetAsync(string ip)
    {
        return Task.CompletedTask;
    }
}