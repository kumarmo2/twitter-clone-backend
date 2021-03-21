using System;
using System.Threading.Tasks;
using CommonLibs.RedisCache;
using StackExchange.Redis;

namespace CommonLibs.RateLimiter.Throttlers
{
    internal class PerSecRequestThrottler : BaseRequestThrottler
    {
        private readonly IRedisCacheManager _redisCacheManager;
        public PerSecRequestThrottler(IRequestThrottler requestThrottler, IRedisCacheManager redisCacheManager) : base(requestThrottler)
        {
            _nextThrottler = requestThrottler;
            _redisCacheManager = redisCacheManager;
        }

        public override async Task<bool> ShouldThrottle(ThrottleRequest throttleRequest)
        {
            if (throttleRequest is null)
            {
                throw new System.ArgumentNullException(nameof(throttleRequest));
            }
            var config = throttleRequest.AppliedConfig;
            var userId = throttleRequest.UserId;
            var now = DateTime.Now;
            var currSec = now.AddMilliseconds(-1 * now.Millisecond);
            var prevSec = currSec.AddSeconds(-1);


            var userRateLimitConfigCacheKey = Utils.GetPerUserPerRateLimitConfigCacheKey(userId, config);
            var hashFields = new string[] { prevSec.ToString(), currSec.ToString() };

            var hashFieldValues = await _redisCacheManager.HashGet(userRateLimitConfigCacheKey, hashFields);
            if (hashFieldValues == null || hashFieldValues.Length < 2)
            {
                Console.WriteLine("some error while fetching values from redis");
                return false;
            }
            var currSecValue = hashFieldValues[1];
            if (currSecValue.IsNull)
            {
                return await HandleCaseWhenCurrSecValueIsNull(throttleRequest, userRateLimitConfigCacheKey, currSec);
            }
            currSecValue.TryParse(out int currSecIntVal);

            var prevSecValue = hashFieldValues[0];
            if (prevSecValue.IsNull)
            {
                return await HandleCaseWhenPrevSecValueIsNull(throttleRequest, userRateLimitConfigCacheKey, currSec, currSecIntVal);
            }
            prevSecValue.TryParse(out int prevSecIntVal);

            return await HandleCaseWhenPrevAndCurrSecHasValue(throttleRequest, userRateLimitConfigCacheKey, now, currSec, currSecIntVal, prevSecIntVal);

        }

        private async Task<bool> HandleCaseWhenPrevAndCurrSecHasValue(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey,
            DateTime now, DateTime currSec, int currSecValue, int prevSecValue)
        {
            if (currSecValue > throttleRequest.AppliedConfig.PerSecLimit)
            {
                return true;
            }
            var currWindow = now - (now.AddSeconds(-1));
            var prevSecPartInCurrWindow = currSec - (now.AddSeconds(-1));
            var prevSecFractionInCurrWindow = prevSecPartInCurrWindow / currWindow;
            var calculatedThrottleLimit = (int)(currSecValue + (prevSecValue * prevSecFractionInCurrWindow));

            if (calculatedThrottleLimit >= throttleRequest.AppliedConfig.PerSecLimit)
            {
                return true;
            }

            if (_nextThrottler == null)
            {
                await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
                return false;
            }
            var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);
            if (!shouldThrottle)
            {
                await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
            }
            return shouldThrottle;
        }

        private async Task<bool> HandleCaseWhenPrevSecValueIsNull(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey, DateTime currSec, int currSecValue)
        {
            if (currSecValue < throttleRequest.AppliedConfig.PerSecLimit)
            {
                if (_nextThrottler == null)
                {
                    await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
                    return false;
                }
                var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);
                if (!shouldThrottle)
                {
                    await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
                }
                return shouldThrottle;
            }
            return true;

        }

        private async Task<bool> HandleCaseWhenCurrSecValueIsNull(ThrottleRequest throttleRequest, string userRateLimitConfigCacheKey, DateTime currSec)
        {
            if (_nextThrottler == null)
            {
                await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
                return false;
            }
            var shouldThrottle = await _nextThrottler.ShouldThrottle(throttleRequest);

            // If the request is not throttled, increase counter for curr sec.
            if (!shouldThrottle)
            {
                await _redisCacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currSec.ToString());
            }
            return shouldThrottle;
        }
    }
}