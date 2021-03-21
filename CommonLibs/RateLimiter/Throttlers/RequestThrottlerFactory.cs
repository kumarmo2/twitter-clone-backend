using System;
using CommonLibs.RedisCache;

namespace CommonLibs.RateLimiter.Throttlers
{
    internal class RequestThrottlerFactory : IRequestThrottlerFactory
    {
        private readonly IRedisCacheManager _redisCacheManager;
        public RequestThrottlerFactory(IRedisCacheManager redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
        }
        public IRequestThrottler GetRequestThrottler(RateLimitConfigOptions config)
        {
            if (config is null)
            {
                throw new System.ArgumentNullException(nameof(config));
            }
            IRequestThrottler perSecThrottler = null;
            if (config.PerSecLimit > 0)
            {
                Console.WriteLine(">>>>>> initializing per sec throttler as well");
                perSecThrottler = new PerSecRequestThrottler(null, _redisCacheManager);
            }

            if (config.PerMinLimit > 0)
            {
                return new PerMinRequestThrottler(perSecThrottler, _redisCacheManager);
            }

            return null;

        }
    }
}