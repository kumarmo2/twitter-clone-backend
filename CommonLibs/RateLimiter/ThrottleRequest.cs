
namespace CommonLibs.RateLimiter
{
    internal class ThrottleRequest
    {
        public RateLimitConfigOptions AppliedConfig { get; set; }
        public string UserId { get; set; }
    }
}