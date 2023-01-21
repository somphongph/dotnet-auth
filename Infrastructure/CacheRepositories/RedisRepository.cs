using System.Text.Json;
using Domain.Interfaces.CacheRepositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.CacheRepositories;

public class RedisRepository : ICacheRepository
{
    private readonly IDistributedCache _distributedCache;

    private readonly IConfiguration _configuration;
    public RedisRepository(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var result = await _distributedCache.GetStringAsync(key);

        if (!String.IsNullOrEmpty(result))
        {
            return JsonSerializer.Deserialize<T>(result);
        }

        return default(T);
    }

    public async Task<T?> AddCacheShortAsync<T>(string key, T data)
    {
        var expireShort = Convert.ToInt32(_configuration.GetValue<string>("RedisSettings:ExpireShort"));

        return await AddCacheAsync(key, data, expireShort);
    }

    public async Task<T?> AddCacheLongAsync<T>(string key, T data)
    {
        var expireLong = Convert.ToInt32(_configuration.GetValue<string>("RedisSettings:ExpireLong"));

        return await AddCacheAsync(key, data, expireLong);
    }

    public async Task<T?> AddCacheAsync<T>(string key, T data, double expires)
    {
        var cache = await _distributedCache.GetStringAsync(key);

        if (String.IsNullOrEmpty(cache))
        {
            var json = JsonSerializer.Serialize(data);
            var opt = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expires));

            await _distributedCache.SetStringAsync(key, json, opt);

            cache = json;
        }

        return JsonSerializer.Deserialize<T>(cache);
    }

    public async Task<T?> AddCacheSlidingAsync<T>(string key, T data, double expires)
    {
        var cache = await _distributedCache.GetStringAsync(key);

        if (String.IsNullOrEmpty(cache))
        {
            var json = JsonSerializer.Serialize(data);
            var opt = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(expires));

            await _distributedCache.SetStringAsync(key, json, opt);

            cache = json;
        }

        return JsonSerializer.Deserialize<T>(cache);
    }

    public async Task RemoveCacheAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}

