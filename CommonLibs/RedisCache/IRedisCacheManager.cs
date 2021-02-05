using System.Threading.Tasks;
using StackExchange.Redis;

namespace CommonLibs.RedisCache
{
    public interface IRedisCacheManager
    {
        // IDatabase GetDatabase();
        Task SetRecord<T>(string key, T value);
        Task<T> GetRecord<T>(string key);
    }
}