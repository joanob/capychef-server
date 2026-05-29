namespace Capychef.Api.Auth;

public class NoOpLoginAttemptTracker : ILoginAttemptTracker
{
    public Task<bool> IsBlockedAsync(string ip) => Task.FromResult(false);
    public Task RecordFailedAttemptAsync(string ip) => Task.CompletedTask;
    public Task ResetAsync(string ip) => Task.CompletedTask;
}

