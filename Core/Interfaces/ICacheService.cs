namespace Core.Interfaces;

public interface ICacheService
{
    Task<T?> SetCacheAsync<T>(string key, T value, TimeSpan expiration);
    Task<T?> GetCacheAsync<T>(string key);
    Task<bool> RemoveCacheAsync(string key);
    Task<bool> IsExistAsync(string key);
    Task<bool> RefreshExpirationAsync(string key, TimeSpan expiration);
    Task<bool> TrySetAsync<T>(string key, T value, TimeSpan expiration) where T : class;
    Task<IReadOnlyList<string>> FindKeysAsync(string pattern);
}