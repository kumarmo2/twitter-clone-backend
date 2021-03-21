using System.Collections.Generic;

namespace CommonLibs.RateLimiter
{
    public interface IRateLimitConfigProvider
    {
        IEnumerable<RateLimitConfigOptions> GetConfig();
    }
}