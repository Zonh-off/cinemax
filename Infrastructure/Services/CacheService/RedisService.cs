using System.Text.Json;
using Core.Common;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastucture.Services.CacheService;

public class RedisService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();
    
    public async Task<T?> SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        var data = await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiration);
        
        if (!data) return null;
        
        return await GetCacheAsync<T>(key);
    }

    public async Task<T?> GetCacheAsync<T>(string key) where T : class
    {
        var data = await _database.StringGetAsync(key);
        
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>(data!);
    }

    public async Task<bool> RemoveCacheAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> IsExistAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }
}