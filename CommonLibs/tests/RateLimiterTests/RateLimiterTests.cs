using System.Threading.Tasks;
using CommonLibs.RateLimiter;
using Moq;
using Xunit;
using CommonLibs.RedisCache;
using System;

namespace CommonLibs.Tests.RateLimterTests
{
    public class RateLimterTests
    {
        [Theory, AutoDomainData]
        public async Task EmptyHttpMethod_ThrowsException(RedisRateLimiter sut)
        {
            // var configProvider = new Mock<IRateLimitConfigProvider>();
            // var cacheManager = new Mock<IRedisCacheManager>();

            // var sut = new RateLimiter.RedisRateLimiter(configProvider.Object,cacheManager.Object);
            await Assert.ThrowsAsync<ArgumentException>(() => sut.ShouldThrottle(null, null, null));

        }
    }
}