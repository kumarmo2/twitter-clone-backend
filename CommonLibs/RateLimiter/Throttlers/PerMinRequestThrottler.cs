using System;
using System.Threading.Tasks;
using CommonLibs.RedisCache;

namespace CommonLibs.RateLimiter.Throttlers
{
    internal sealed class PerMinRequestThrottler : BaseRequestThrottler
    {
        private readonly IRedisCacheManager _cacheManager;
        public PerMinRequestThrottler(IRequestThrottler requestThrottler, IRedisCacheManager cacheManager) : base(requestThrottler)
        {
            _cacheManager = cacheManager;
        }

        public override async Task<bool> ShouldThrottle(ThrottleRequest throttleRequest)
        {
            if (throttleRequest is null)
            {
                throw new System.ArgumentNullException(nameof(throttleRequest));
            }
            var configOption = throttleRequest.AppliedConfig;
            var userId = throttleRequest.UserId;

            var perMinLimit = configOption.PerMinLimit;
            var perSecLimit = configOption.PerSecLimit;
            Console.WriteLine($"perMinLimit: {perMinLimit}");

            var userRateLimitConfigCacheKey = Utils.GetPerUserPerRateLimitConfigCacheKey(userId, configOption);

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
                return await HandleCaseWhenCurrMinValueIsNull(throttleRequest, userRateLimitConfigCacheKey, currMin);
            }
            currMinValue.TryParse(out int currMinIntegerValue);
            var prevMinValue = values[0];
            if (prevMinValue.IsNull)
            {
                return await HandleCaseWhenPrevMinValueIsNull(throttleRequest, userRateLimitConfigCacheKey, currMin, currMinIntegerValue);
            }
            prevMinValue.TryParse(out int prevMinIntergerValue);
            return await HandleCaseWhenPrevAndCurrMinHasValue(throttleRequest, userRateLimitConfigCacheKey, now, currMin,
                currMinIntegerValue, prevMinIntergerValue);
        }

        private async Task<bool> HandleCaseWhenPrevAndCurrMinHasValue(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey, DateTime now, DateTime currMin,
            int currMinIntegerValue, int prevMinIntergerValue)
        {
            if (currMinIntegerValue >= throttleRequest.AppliedConfig.PerMinLimit)
            {
                return true;
            }
            var currWindow = now - now.AddMinutes(-1);
            var prevMinPartInCurrWindow = currMin - (now.AddMinutes(-1));
            var prevMinFractionalPartInCurrWindow = prevMinPartInCurrWindow / currWindow;

            var calculatedvalue = (int)(currMinIntegerValue + prevMinIntergerValue * prevMinFractionalPartInCurrWindow);
            if (calculatedvalue >= throttleRequest.AppliedConfig.PerMinLimit)
            {
                return true;
            }

            if (_nextThrottler == null)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                return false;
            }
            var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);
            if (!shouldThrottle)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
            }
            return shouldThrottle;
        }

        private async Task<bool> HandleCaseWhenPrevMinValueIsNull(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey, DateTime currMin, int currMinValue)
        {
            if (currMinValue >= throttleRequest.AppliedConfig.PerMinLimit)
            {
                return true;
            }
            if (_nextThrottler == null)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                return false;
            }
            var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);
            if (!shouldThrottle)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMinValue.ToString());
            }
            return shouldThrottle;
        }

        private async Task<bool> HandleCaseWhenCurrMinValueIsNull(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey, DateTime currMin)
        {
            if (_nextThrottler == null)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                return false;
            }
            var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);
            if (!shouldThrottle)
            {
                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
            }
            return shouldThrottle;

        }
    }
}