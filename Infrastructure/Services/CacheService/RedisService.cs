using System.Text.Json;
using Core.Common;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastucture.Services.CacheService;

public class RedisService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();
    
    public async Task<T?> SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        var data = await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiration);
        
        if (!data) return default;
        
        return await GetCacheAsync<T>(key);
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var data = await _database.StringGetAsync(key);
        
        return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data!);
    }

    public async Task<bool> RemoveCacheAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> IsExistAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    public async Task<bool> RefreshExpirationAsync(string key, TimeSpan expiration)
    {
        return await _database.KeyExpireAsync(key, expiration);
    }

    public async Task<bool> TrySetAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        return await _database.StringSetAsync(
            key,
            JsonSerializer.Serialize(value),
            expiration,
            When.NotExists
        );
    }

    public async Task<IReadOnlyList<string>> FindKeysAsync(string pattern)
    {
        var server = GetServer();
        var keys = new List<string>();

        await foreach (var key in server.KeysAsync(pattern: pattern))
        {
            keys.Add(key.ToString());
        }

        return keys;
    }

    private IServer GetServer()
    {
        var endpoints = redis.GetEndPoints();
        if (endpoints.Length == 0)
            throw new Exception("No Redis endpoints configured");

        return redis.GetServer(endpoints.First());
    }
}