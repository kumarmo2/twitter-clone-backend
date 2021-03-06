using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibs.RedisCache;
namespace CommonLibs.RateLimiter
{
    public class RedisRateLimiter : IRateLimiter
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
                var configOptions = _configProvider.GetConfig();
                return configOptions.ToDictionary(option => Utils.GetRateLimitConfigUniqueKey(option));
            });
        }

        public async Task<bool> ShouldThrottle(string httpMethod, string fullPath, string userId)
        {
            // TODO: need to think about the endsWithPath approach.
            // if in the config, someone adds "/" as endsWiPath, which is basically catch all. so how to prevent this?
            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentException($"'{nameof(httpMethod)}' cannot be null or empty.", nameof(httpMethod));
            }

            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentException($"'{nameof(fullPath)}' cannot be null or whitespace.", nameof(fullPath));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or whitespace.", nameof(userId));
            }
            // TODO: optimize this searching.
            var endsWithPath = _configDictionary.Value.Keys.FirstOrDefault(endsWithPath => fullPath.EndsWith(endsWithPath));
            if (string.IsNullOrWhiteSpace(endsWithPath))
            {
                return false;
            }
            _configDictionary.Value.TryGetValue(endsWithPath, out var configOption);
            if (configOption == null)
            {
                return false;
            }
            var perMinLimit = configOption.PerMinLimit;
            var perSecLimit = configOption.PerSecLimit;

            var userRateLimitConfigCacheKey = Utils.GetPerUserPerRateLimitConfigCacheKey(userId, configOption);

            if (perMinLimit > 0)
            {
                var now = DateTime.Now;
                var currMin = now.AddSeconds(-1 * now.Second);
                var prevMin = currMin.AddMinutes(-1);
                var hashFields = new string[] { prevMin.ToString(), currMin.ToString() };
                var values = await _cacheManager.HashGet(userRateLimitConfigCacheKey, hashFields);
                if (values == null || values.Length < 2)
                {
                    Console.WriteLine("some error while retriving date from redis. Allowing the request to not throttle. But look into this");
                    return false;
                }
                var currMinValue = values[1];
                if (currMinValue.IsNull)
                {
                    // TODO: 
                    // increment counter for current min and current second.
                    return false;
                }
                currMinValue.TryParse(out int currMinIntegerValue);
                if (currMinIntegerValue >= perMinLimit)
                {
                    return true;
                }
                var prevMinValue = values[0];
                if (prevMinValue.IsNull)
                {
                    // Increment counter for current min and curr second.
                    return false;
                }
                prevMinValue.TryParse(out int prevMinIntergerValue);
                var currWindowStart = now.AddMinutes(-1);

                var x = currMin - prevMin;
                var y = currMin - currWindowStart;

                var calculatedvalue = (int)((y / x) * prevMinIntergerValue + currMinIntegerValue);
                if (calculatedvalue >= perMinLimit)
                {
                    return true;
                }
                // if we reach this point that means this request has passed the  perMinLimit


                // TODO: implement logic for throttling on per second limit.

                return false;
            }
            return false;


        }
    }
}