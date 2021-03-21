namespace CommonLibs.RateLimiter
{
    public class RateLimitConfigOptions
    {
        public string HttpMethod { get; set; }
        public string EndsWithPath { get; set; }
        public int PerSecLimit { get; set; }
        public int PerMinLimit { get; set; }
    }
}