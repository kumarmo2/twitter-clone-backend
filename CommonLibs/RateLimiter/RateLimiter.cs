using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibs.RedisCache;
namespace CommonLibs.RateLimiter
{
    public class RedisRateLimiter
    {
        private IRateLimitConfigProvider _configProvider;
        private IRedisCacheManager _cacheManager;
        private Lazy<Dictionary<string, RateLimitConfigOptions>> _configDictionary;

        public RedisRateLimiter(IRateLimitConfigProvider configProvider, IRedisCacheManager cacheManager)
        {
            _configProvider = configProvider;
            _cacheManager = cacheManager;
            _configDictionary = new Lazy<Dictionary<string, RateLimitConfigOptions>>(() =>
            {
                // It is the responsibility of the configProvider that it doesn't give multiple same configs.
                var configOptions = _configProvider.GeConfig();
                return configOptions.ToDictionary(option => Utils.GetRateLimitConfigUniqueKey(option));
            });
        }

        public async Task<bool> ShouldThrottle(string httpMethod, string fullPath, string userId)
        {
            var endsWithPath = _configDictionary.Value.Keys.FirstOrDefault(endsWithPath => fullPath.Contains(endsWithPath));
            if (string.IsNullOrWhiteSpace(endsWithPath))
            {
                return false;
            }
            // TODO: implement logic for throttling.

            return false;
        }


    }
}