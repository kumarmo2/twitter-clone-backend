using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLibs.RateLimiter.Extensions
{
    public static class ServiceCollectionExtension
    {
        /*
        * I have kept AddLocalRateLimitConfigProvider and AddRateLimiter separate instead of keeping the addition of
        ConfigProvider inside the `AddRateLimiter` only, because in future, we might get new way of getting Config, example
        could be from some api.

        */
        public static void AddLocalRateLimitConfigProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRateLimitConfigProvider>(new LocalRateLimitConfigProvider(configuration));
        }

        public static void AddRateLimiter(this IServiceCollection services)
        {
            services.AddSingleton<IRateLimiter, RedisRateLimiter>();
        }

    }
}