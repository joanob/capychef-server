using Capychef.Api.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Capychef.Api.Realtime;

public class RealtimeHub(IConnectionMappingStore connections) : Hub
{
    public override Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();

        if (context == null) return base.OnConnectedAsync();

        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(context);

        if (userDetails.IsValid())
        {
            var connectionDetails = new ConnectionDetails
                (Context.ConnectionId, userDetails);

            _ = connections.AddAsync(connectionDetails);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _ = connections.RemoveAsync(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}