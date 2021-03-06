namespace CommonLibs.RateLimiter
{
    public static class Utils
    {
        public static string GetRateLimitConfigUniqueKey(RateLimitConfigOptions config) => GetRateLimitConfigUniqueKey(config.HttpMethod, config.EndsWithPath);
        public static string GetRateLimitConfigUniqueKey(string httpMethod, string endsWithPath) => $"method:{httpMethod.ToLowerInvariant()}:endswithpath:{endsWithPath.ToLowerInvariant()}";
        public static string GetPerUserPerRateLimitConfigCacheKey(string userId, RateLimitConfigOptions config) => $"user:{userId}{GetRateLimitConfigUniqueKey(config)}";
    }
}