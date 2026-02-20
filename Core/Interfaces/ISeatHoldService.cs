namespace Core.Interfaces;

public interface ISeatHoldService
{
    Task<(bool ok, string? reason)> HoldSeatAsync(int showtimeId, int seatId, string userId, TimeSpan ttl);
    Task<(bool ok, int? failedSeatId, string? reason)> HoldSeatsAsync(int showtimeId, IReadOnlyList<int> seatIds, string userId, TimeSpan ttl);
    Task ReleaseSeatAsync(int showtimeId, int seatId, string userId);
    Task<bool> IsHeldByUserAsync(int showtimeId, int seatId, string userId);
    Task<Dictionary<int, string>> GetHeldSeatsAsync(int showtimeId);
    Task ClearUserHoldsAsync(int showtimeId, string userId);
    Task<string?> GetSeatHolderAsync(int showtimeId, int seatId);
    Task<IReadOnlyList<int>> RefreshHoldsAsync(int showtimeId, IReadOnlyList<int> seatIds, string userId, TimeSpan ttl);
    Task<IReadOnlyList<int>> ReleaseAllUserHoldsAsync(int showtimeId, string userId);
}