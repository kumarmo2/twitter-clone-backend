namespace CommonLibs.RateLimiter.Throttlers
{
    internal interface IRequestThrottlerFactory
    {
        IRequestThrottler GetRequestThrottler(RateLimitConfigOptions config);
    }
}