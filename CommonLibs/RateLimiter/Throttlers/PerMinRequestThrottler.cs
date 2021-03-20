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
                if (_nextThrottler == null)
                {

                    await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                    // increment counter for current min and current second(only if there is a rule for per seconds too)
                    return false;
                }
                else
                {

                    // TODO: before incrementing, call should Throttole on nextRequestThrottler if exists
                    // if there is no next throttler, increase the counter and return
                    // if there is nextThrottler and it returns true for shouldThrottle, don't increment counter and return true.
                }

            }
            currMinValue.TryParse(out int currMinIntegerValue);
            if (currMinIntegerValue >= perMinLimit)
            {
                return true;
            }
            var prevMinValue = values[0];
            if (prevMinValue.IsNull)
            {
                // TODO: before incrementing, call should Throttole on nextRequestThrottler if exists
                // if there is no next throttler, increase the counter and return
                // if there is nextThrottler and it returns true for shouldThrottle, don't increment counter and return true.
                if (_nextThrottler == null)
                {

                    await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                    // TODO: 
                    // increment counter for current min and current second(only if there is a rule for per seconds too)
                    return false;
                }
                else
                {

                    // TODO: before incrementing, call should Throttole on nextRequestThrottler if exists
                    // if there is no next throttler, increase the counter and return
                    // if there is nextThrottler and it returns true for shouldThrottle, don't increment counter and return true.
                }
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
            // TODO: we should only increment the counter if there are no further rules to counter.
            // TODO: before incrementing, call should Throttole on nextRequestThrottler if exists
            // if there is no next throttler, increase the counter and return
            // if there is nextThrottler and it returns true for shouldThrottle, don't increment counter and return true.
            if (_nextThrottler == null)
            {

                await _cacheManager.HashIncrementAsync(userRateLimitConfigCacheKey, currMin.ToString());
                return false;
            }
            else
            {

                // TODO: before incrementing, call should Throttole on nextRequestThrottler if exists
                // if there is no next throttler, increase the counter and return
                // if there is nextThrottler and it returns true for shouldThrottle, don't increment counter and return true.
            }
            return false;

        }
    }
}