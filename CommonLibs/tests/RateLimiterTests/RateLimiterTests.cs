using System.Threading.Tasks;
using CommonLibs.RateLimiter;
using Xunit;
using System;

namespace CommonLibs.Tests.RateLimterTests
{
    public class RateLimterTests
    {
        // TODO: need to figure out how to test internal components.
        // [Theory, AutoDomainData]
        // public async Task EmptyHttpMethod_ThrowsException(RedisRateLimiter sut)
        // {
        //     // var configProvider = new Mock<IRateLimitConfigProvider>();
        //     // var cacheManager = new Mock<IRedisCacheManager>();

        //     // var sut = new RateLimiter.RedisRateLimiter(configProvider.Object,cacheManager.Object);
        //     await Assert.ThrowsAsync<ArgumentException>(() => sut.ShouldThrottle(null, null, null));

        // }
    }
}