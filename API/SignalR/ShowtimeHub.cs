using System.Collections.Concurrent;
using System.Security.Claims;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class ShowtimeHub(ISeatHoldService holds) : Hub
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<int, byte>> _connShowtimes = new();

    private string UserId =>
        Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new HubException("Unauthorized: missing user id");

    private void TrackShowtime(int showtimeId)
    {
        var set = _connShowtimes.GetOrAdd(Context.ConnectionId, _ => new ConcurrentDictionary<int, byte>());
        set.TryAdd(showtimeId, 0);
    }

    private void UntrackShowtime(int showtimeId)
    {
        if (_connShowtimes.TryGetValue(Context.ConnectionId, out var set))
        {
            set.TryRemove(showtimeId, out _);
            if (set.IsEmpty) _connShowtimes.TryRemove(Context.ConnectionId, out _);
        }
    }

    public async Task JoinShowtime(int showtimeId)
    {
        TrackShowtime(showtimeId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");
    }

    public async Task LeaveShowtime(int showtimeId)
    {
        UntrackShowtime(showtimeId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"showtime-{showtimeId}");
    }

    public async Task HoldSeat(int showtimeId, int seatId)
    {
        await JoinShowtime(showtimeId);

        var (ok, reason) = await holds.HoldSeatAsync(showtimeId, seatId, UserId, TimeSpan.FromMinutes(5));
        if (!ok) throw new HubException(reason ?? "Seat already held");

        await Clients.Group($"showtime-{showtimeId}")
            .SendAsync("SeatHeld", new { seatId, userId = UserId });
    }

    public async Task HoldMany(int showtimeId, List<int> seatIds)
    {
        await JoinShowtime(showtimeId);

        var (ok, failedSeatId, reason) =
            await holds.HoldSeatsAsync(showtimeId, seatIds, UserId, TimeSpan.FromMinutes(5));

        if (!ok)
            throw new HubException($"Failed to hold seat {failedSeatId}. Reason: {reason}");

        await Clients.Group($"showtime-{showtimeId}")
            .SendAsync("SeatsHeld", new { seatIds = seatIds.Distinct().ToList(), userId = UserId });
    }

    public async Task ReleaseSeat(int showtimeId, int seatId)
    {
        await holds.ReleaseSeatAsync(showtimeId, seatId, UserId);

        await Clients.Group($"showtime-{showtimeId}")
            .SendAsync("SeatReleased", new { seatId });
    }

    public async Task Heartbeat(int showtimeId, List<int> seatIds)
    {
        TrackShowtime(showtimeId);

        var stillHeld = await holds.RefreshHoldsAsync(showtimeId, seatIds, UserId, TimeSpan.FromMinutes(5));

        await Clients.Caller.SendAsync("HeartbeatAck", new
        {
            showtimeId,
            stillHeldSeatIds = stillHeld
        });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connShowtimes.TryRemove(Context.ConnectionId, out var showtimes))
        {
            foreach (var showtimeId in showtimes.Keys)
            {
                var released = await holds.ReleaseAllUserHoldsAsync(showtimeId, UserId);

                if (released.Count > 0)
                {
                    await Clients.Group($"showtime-{showtimeId}")
                        .SendAsync("SeatsReleased", new { seatIds = released });
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
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