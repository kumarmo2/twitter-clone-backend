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
                // It is the responsibility of the configProvider that it doesn't give multiple same configs
                // or else this will fail.
                // TODO: make initialization resilient to this above scenario.
                var configOptions = _configProvider.GeConfig();
                return configOptions.ToDictionary(option => Utils.GetRateLimitConfigUniqueKey(option));
            });
        }

        public async Task<bool> ShouldThrottle(string httpMethod, string fullPath, string userId)
        {
            // TODO: optimize this searching.
            var endsWithPath = _configDictionary.Value.Keys.FirstOrDefault(endsWithPath => fullPath.Contains(endsWithPath));
            if (string.IsNullOrWhiteSpace(endsWithPath))
            {
                return false;
            }
            _configDictionary.Value.TryGetValue(endsWithPath, out var configOption);
            if (configOption == null)
            {
                return false;
            }

            // TODO: implement logic for throttling.

            return false;
        }


    }
}