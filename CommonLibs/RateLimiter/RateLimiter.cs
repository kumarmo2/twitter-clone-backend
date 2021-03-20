using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibs.RateLimiter.Throttlers;
using CommonLibs.RedisCache;
namespace CommonLibs.RateLimiter
{
    internal class RedisRateLimiter : IRateLimiter
    {
        private IRateLimitConfigProvider _configProvider;
        private IRedisCacheManager _cacheManager;
        private Lazy<Dictionary<string, RateLimitConfigOptions>> _configDictionary;
        private readonly IRequestThrottlerFactory _requestThrottlerFactory;

        public RedisRateLimiter(IRateLimitConfigProvider configProvider, IRedisCacheManager cacheManager,
        IRequestThrottlerFactory requestThrottlerFactory)
        {
            _configProvider = configProvider;
            _cacheManager = cacheManager;
            _requestThrottlerFactory = requestThrottlerFactory;
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
            // TODO: needs major REFACTORING.
            // TODO: need to think about the endsWithPath approach.
            // if in the config, someone adds "/" as endsWiPath, which is basically catch all. so how to prevent this?
            Console.WriteLine("should throttle called");
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
            Console.WriteLine($"fullPath: {fullPath}");
            var configOption = _configDictionary.Value.Values.FirstOrDefault(config =>
            {
                // Console.WriteLine($"config key: {}");
                return fullPath.EndsWith(config.EndsWithPath);
            });
            if (configOption == null)
            {
                return false;
            }
            var throttler = _requestThrottlerFactory.GetRequestThrottler(configOption);
            if (throttler != null)
            {
                return await throttler.ShouldThrottle(new ThrottleRequest
                {
                    AppliedConfig = configOption,
                    UserId = userId
                });
            }
            return false;
        }
    }
}