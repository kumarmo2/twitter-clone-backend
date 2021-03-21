using System.Threading.Tasks;

namespace CommonLibs.RateLimiter.Throttlers
{
    internal interface IRequestThrottler
    {
        Task<bool> ShouldThrottle(ThrottleRequest throttleRequest);
    }
}