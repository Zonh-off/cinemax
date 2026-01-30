using Core.Common;

namespace Core.Interfaces;

public interface ICacheService
{
    Task<T?> SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class;
    Task<T?> GetCacheAsync<T>(string key) where T : class;
    Task<bool> RemoveCacheAsync(string key);
    Task<bool> IsExistAsync(string key);
}