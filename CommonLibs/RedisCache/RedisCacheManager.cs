using StackExchange.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommonLibs.RedisCache
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly Lazy<ConnectionMultiplexer> _redis;
        public RedisCacheManager(IOptions<RedisOptions> redisOptions)
        {
            _redis = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString), true);
        }

        // public IDatabase GetDatabase() => _redis.Value.GetDatabase();

        public async Task<T> GetRecord<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
            }
            var cache = _redis.Value.GetDatabase();
            var redisValue = await cache.StringGetAsync(key);

            if (redisValue.IsNull)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(redisValue.ToString());
        }

        public async Task SetRecord<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
            var json = JsonConvert.SerializeObject(value);

            var cache = _redis.Value.GetDatabase();
            await cache.StringSetAsync(key, json);
        }
    }
}