
using System.Text.Json;
using StackExchange.Redis;

namespace CachingWebApi.Service;

public class CacheService : ICacheService
{
    private IDatabase _cacheDb;

    public CacheService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        _cacheDb = redis.GetDatabase();
    }

    public T GetData<T>(string key)
    {
        var _value = _cacheDb.StringGet(key);
        if (!string.IsNullOrEmpty(_value))
        {
            return JsonSerializer.Deserialize<T>(_value);
        }
        return default;
    }

    public object RemoveData(string key)
    {
        var _exist = _cacheDb.KeyExists(key);
        if (_exist)
        {
            return _cacheDb.KeyDelete(key);
        }
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var _expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), _expirtyTime);
    }
}
