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
                // TODO:
                /*
                    To handle if PerSecLimit > 0
                */
            }

            if (config.PerMinLimit > 0)
            {
                return new PerMinRequestThrottler(perSecThrottler, _redisCacheManager);
            }

            return null;

        }
    }
}