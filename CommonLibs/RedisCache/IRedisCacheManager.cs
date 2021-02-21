using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CommonLibs.RedisCache
{
    public interface IRedisCacheManager
    {
        IDatabase GetDatabase();
        Task SetRecord<T>(string key, T value);
        Task<T> GetRecord<T>(string key);
        Task RemoveKey(string key);
        Task<long> ListLeftPush<T>(string key, T value);
        Task<List<T>> ListRange<T>(string key, long start = 0, long stop = -1);
        Task<long> ListRemove<T>(string key, T payload, long count = 0);
        Task SortedSetAdd<T>(string key, T value, long score);
        Task<long> SortedSetRemoveRangeByScore(string key, long start, long stop);
    }
}