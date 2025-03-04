using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _tracker;

    public PresenceHub(PresenceTracker tracker)
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        var isOnline = await _tracker.UserConnectedAsync(Context.User.GetUsername(), Context.ConnectionId);
        if (isOnline)
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

        var currentUsers = await _tracker.GetOnlineUsersAsync();
        await Clients.Caller.SendAsync("GetOnlineUsersAsync", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var isOffline = await _tracker.UserDisconnectedAsync(Context.User.GetUsername(), Context.ConnectionId);

        if (isOffline)
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

        await base.OnDisconnectedAsync(exception);
    }
}
