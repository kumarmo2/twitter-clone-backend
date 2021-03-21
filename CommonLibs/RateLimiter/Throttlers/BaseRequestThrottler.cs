using System.Threading.Tasks;

namespace CommonLibs.RateLimiter.Throttlers
{
    abstract class BaseRequestThrottler : IRequestThrottler
    {
        protected internal BaseRequestThrottler(IRequestThrottler requestThrottler)
        {
            _nextThrottler = requestThrottler;
        }
        private protected IRequestThrottler _nextThrottler;
        public virtual void SetNextThrottler(IRequestThrottler nextThrottler)
        {
            _nextThrottler = nextThrottler;
        }
        public abstract Task<bool> ShouldThrottle(ThrottleRequest throttleRequest);
    }
}