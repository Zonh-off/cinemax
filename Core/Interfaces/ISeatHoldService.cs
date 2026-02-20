namespace Core.Interfaces;

public interface ISeatHoldService
{
    Task<(bool ok, string? reason)> HoldSeatAsync(int showtimeId, int seatId, string userId, TimeSpan ttl);
    Task ReleaseSeatAsync(int showtimeId, int seatId, string userId);

    Task<bool> IsHeldByUserAsync(int showtimeId, int seatId, string userId);
    Task<Dictionary<int, string>> GetHeldSeatsAsync(int showtimeId); // seatId -> userId
    Task ClearUserHoldsAsync(int showtimeId, string userId);
}