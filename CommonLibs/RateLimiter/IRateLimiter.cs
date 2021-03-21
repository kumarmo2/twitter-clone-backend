using System.Threading.Tasks;

namespace CommonLibs.RateLimiter
{
    public interface IRateLimiter
    {
        Task<bool> ShouldThrottle(string httpMethod, string fullPath, string userId);

    }
}