using StackExchange.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibs.RedisCache
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly Lazy<ConnectionMultiplexer> _redis;
        public RedisCacheManager(IOptions<RedisOptions> redisOptions)
        {
            _redis = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString), true);
        }

        public IDatabase GetDatabase() => _redis.Value.GetDatabase();

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

        public async Task<RedisValue[]> HashGet(string key, string[] hasFields)
        {
            var cache = _redis.Value.GetDatabase();

            var fields = hasFields.Select(field => new RedisValue(field)).ToArray();
            var values = await cache.HashGetAsync(key, fields);
            return values;
        }

        // Returns length of the list
        public async Task<long> ListLeftPush<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
            var cache = _redis.Value.GetDatabase();
            var json = JsonConvert.SerializeObject(value);
            return await cache.ListLeftPushAsync(key, json);
        }

        public async Task<List<T>> ListRange<T>(string key, long start = 0, long stop = -1)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
            var cache = _redis.Value.GetDatabase();
            var redisList = await cache.ListRangeAsync(key, start, stop);

            // When key doesn't exist, redis returns an empty array.
            return redisList
                        .Select(redisValue => JsonConvert.DeserializeObject<T>(redisValue))
                        .ToList();
        }

        public async Task<long> ListRemove<T>(string key, T payload, long count = 0)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var cache = _redis.Value.GetDatabase();
            var json = JsonConvert.SerializeObject(payload);
            return await cache.ListRemoveAsync(key, json);
        }

        public async Task RemoveKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var cache = _redis.Value.GetDatabase();
            await cache.KeyDeleteAsync(key);
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

        public async Task SortedSetAdd<T>(string key, T value, long score)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var json = JsonConvert.SerializeObject(value);
            var cache = _redis.Value.GetDatabase();

            await cache.SortedSetAddAsync(key, json, score);
        }

        public async Task<long> SortedSetRemoveRangeByScore(string key, long start, long stop)
        {
            var cache = _redis.Value.GetDatabase();
            return await cache.SortedSetRemoveRangeByScoreAsync(key, start, stop);
        }
    }
}