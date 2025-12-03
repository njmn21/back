using Microsoft.Extensions.Caching.Memory;

namespace back.Utils
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public void Set<T>(string key, T data, TimeSpan duration)
        {
            _cache.Set(key, data, duration);
            _logger.LogInformation("Cache set for key: {Key} with duration: {Duration}", key, duration);
        }

        public bool TryGet<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _logger.LogInformation("Cache removed for key: {Key}", key);
        }

        public void RemoveMany(params string[] keys)
        {
            foreach (var key in keys)
            {
                _cache.Remove(key);
                _logger.LogInformation("Cache removed for key: {Key}", key);
            }
        }
    }
}
