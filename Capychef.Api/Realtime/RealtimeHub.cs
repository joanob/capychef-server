using Capychef.Api.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Capychef.Api.Realtime;

public class RealtimeHub : Hub
{
    private readonly IConnectionMappingStore _connections;

    public RealtimeHub(IConnectionMappingStore connections)
    {
        _connections = connections;
    }

    public override Task OnConnectedAsync()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(Context.GetHttpContext());

        if (userDetails != null)
        {
            var connectionDetails = new ConnectionDetails
                (Context.ConnectionId, userDetails);

            _ = _connections.AddAsync(connectionDetails);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _ = _connections.RemoveAsync(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}