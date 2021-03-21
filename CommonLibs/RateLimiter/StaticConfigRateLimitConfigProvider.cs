using System.Collections.Generic;

namespace CommonLibs.RateLimiter
{

    class StaticConfigRateLimitConfigProvider : IRateLimitConfigProvider
    {
        public IEnumerable<RateLimitConfigOptions> GetConfig()
        {
            return new List<RateLimitConfigOptions>()
            {
                new RateLimitConfigOptions
                {
                    EndsWithPath = "tweet",
                    HttpMethod = "post",
                    PerMinLimit = 9,
                    PerSecLimit = 2,
                }
            };

        }
    }
}