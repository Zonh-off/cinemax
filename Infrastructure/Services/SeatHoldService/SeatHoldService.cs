using Core.Interfaces;

namespace Infrastucture.Services.SeatHoldService;

public class SeatHoldService : ISeatHoldService
{
    private readonly ICacheService _cache;

    public SeatHoldService(ICacheService cacheService)
    {
        _cache = cacheService;
    }

    private static string Key(int showtimeId, int seatId) => $"hold:{showtimeId}:{seatId}";
    private static string Pattern(int showtimeId) => $"hold:{showtimeId}:*";

    public async Task<(bool ok, string? reason)> HoldSeatAsync(int showtimeId, int seatId, string userId, TimeSpan ttl)
    {
        var key = Key(showtimeId, seatId);

        if (await _cache.IsExistAsync(key))
        {
            var existingUser = await _cache.GetCacheAsync<string>(key);

            if (existingUser == userId)
            {
                await _cache.RefreshExpirationAsync(key, ttl);
                return (true, null);
            }

            return (false, "Seat already held");
        }

        var ok = await _cache.TrySetAsync(key, userId, ttl);
        return ok ? (true, null) : (false, "Seat already held");
    }

    public async Task<(bool ok, int? failedSeatId, string? reason)> HoldSeatsAsync(
        int showtimeId, IReadOnlyList<int> seatIds, string userId, TimeSpan ttl)
    {
        var unique = seatIds.Distinct().ToList();
        var heldNow = new List<int>();

        foreach (var seatId in unique)
        {
            var (ok, reason) = await HoldSeatAsync(showtimeId, seatId, userId, ttl);
            if (!ok)
            {
                foreach (var s in heldNow)
                    await ReleaseSeatAsync(showtimeId, s, userId);

                return (false, seatId, reason ?? "Seat already held");
            }

            heldNow.Add(seatId);
        }

        return (true, null, null);
    }

    public async Task<bool> IsHeldByUserAsync(int showtimeId, int seatId, string userId)
    {
        var existingUser = await _cache.GetCacheAsync<string>(Key(showtimeId, seatId));
        return existingUser == userId;
    }

    public async Task<Dictionary<int, string>> GetHeldSeatsAsync(int showtimeId)
    {
        var keys = await _cache.FindKeysAsync(Pattern(showtimeId));

        var result = new Dictionary<int, string>(keys.Count);
        foreach (var key in keys)
        {
            var userId = await _cache.GetCacheAsync<string>(key);
            if (string.IsNullOrEmpty(userId)) continue;

            // key format: hold:{showtimeId}:{seatId}
            var parts = key.Split(':');
            if (parts.Length != 3) continue;
            if (!int.TryParse(parts[2], out var seatId)) continue;

            result[seatId] = userId;
        }

        return result;
    }

    public async Task ClearUserHoldsAsync(int showtimeId, string userId)
    {
        var keys = await _cache.FindKeysAsync(Pattern(showtimeId));
        foreach (var key in keys)
        {
            var existingUser = await _cache.GetCacheAsync<string>(key);
            if (existingUser == userId)
                await _cache.RemoveCacheAsync(key);
        }
    }
    
     public Task<string?> GetSeatHolderAsync(int showtimeId, int seatId)
        => _cache.GetCacheAsync<string>(Key(showtimeId, seatId));

    public async Task<IReadOnlyList<int>> RefreshHoldsAsync(
        int showtimeId, IReadOnlyList<int> seatIds, string userId, TimeSpan ttl)
    {
        var okSeats = new List<int>();
        foreach (var seatId in seatIds.Distinct())
        {
            var holder = await GetSeatHolderAsync(showtimeId, seatId);
            if (holder == userId)
            {
                await _cache.RefreshExpirationAsync(Key(showtimeId, seatId), ttl);
                okSeats.Add(seatId);
            }
        }
        return okSeats;
    }

    public async Task ReleaseSeatAsync(int showtimeId, int seatId, string userId)
    {
        var key = Key(showtimeId, seatId);

        var existingUser = await _cache.GetCacheAsync<string>(key);
        if (existingUser == null) return;

        if (existingUser == userId)
            await _cache.RemoveCacheAsync(key);
    }
    
    public async Task<IReadOnlyList<int>> ReleaseAllUserHoldsAsync(int showtimeId, string userId)
    {
        var keys = await _cache.FindKeysAsync(Pattern(showtimeId));
        var released = new List<int>();

        foreach (var key in keys)
        {
            var existingUser = await _cache.GetCacheAsync<string>(key);
            if (existingUser != userId) continue;

            // key: hold:{showtimeId}:{seatId}
            var parts = key.Split(':');
            if (parts.Length == 3 && int.TryParse(parts[2], out var seatId))
                released.Add(seatId);

            await _cache.RemoveCacheAsync(key);
        }

        return released;
    }
}