using System.Security.Claims;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class ShowtimeHub(ISeatHoldService holds) : Hub
{
    public Task JoinShowtime(int showtimeId)
        => Groups.AddToGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");

    public Task LeaveShowtime(int showtimeId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");

    public async Task HoldSeat(int showtimeId, int seatId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            throw new HubException("Unauthorized: missing user id");

        var (ok, reason) = await holds.HoldSeatAsync(showtimeId, seatId, userId, TimeSpan.FromMinutes(5));
        if (!ok) throw new HubException(reason ?? "Seat already held");

        await Clients.Group($"showtime-{showtimeId}")
           .SendAsync("SeatHeld", new { seatId, userId });
    }

    public async Task ReleaseSeat(int showtimeId, int seatId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            throw new HubException("Unauthorized: missing user id");

        await holds.ReleaseSeatAsync(showtimeId, seatId, userId);

        await Clients.Group($"showtime-{showtimeId}")
           .SendAsync("SeatReleased", new { seatId });
    }
}

public class ShowtimeNotifier : IShowtimeNotifier
{
    private readonly IHubContext<ShowtimeHub> _hub;

    public ShowtimeNotifier(IHubContext<ShowtimeHub> hub)
    {
        _hub = hub;
    }

    public Task SeatsConfirmedAsync(int showtimeId, IReadOnlyList<int> seatIds)
        => _hub.Clients.Group($"showtime-{showtimeId}")
           .SendAsync("SeatsConfirmed", new { seatIds });
}