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

    public T Set<T>(string key, T value, TimeSpan timeout) where T : class
    {
        var timeoutOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = timeout
        };

        _cache.SetString(key, JsonConvert.SerializeObject(value), timeoutOptions);

        return Get<T>(key);
    }
}

