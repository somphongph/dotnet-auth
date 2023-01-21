namespace Domain.Interfaces.CacheRepositories;

public interface ICacheRepository
{
    Task<T?> GetCacheAsync<T>(string key);
    Task<T?> AddCacheShortAsync<T>(string key, T data);
    Task<T?> AddCacheLongAsync<T>(string key, T data);
    Task<T?> AddCacheAsync<T>(string key, T data, double expires);
    Task<T?> AddCacheSlidingAsync<T>(string key, T data, double expires);
    Task RemoveCacheAsync(string key);
}
