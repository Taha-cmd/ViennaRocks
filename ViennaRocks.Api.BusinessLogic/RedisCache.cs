using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViennaRocks.Api.BusinessLogic.Contract;

namespace ViennaRocks.Api.BusinessLogic;

public class RedisCache : ICache
{
    private readonly IDistributedCache _cache;

    public RedisCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T Get<T>(string key) where T : class
    {
        string value = _cache.GetString(key);
        return value is not null ? JsonConvert.DeserializeObject<T>(value) : null;
    }

    public T Set<T>(string key, T value) where T : class
    {
        var timeout = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };

        _cache.SetString(key, JsonConvert.SerializeObject(value), timeout);

        return Get<T>(key);
    }
}

