using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CommonLibs.RateLimiter
{
    public class LocalRateLimitConfigProvider : IRateLimitConfigProvider
    {
        private readonly Lazy<IConfiguration> _rateLimitConfigSection;
        private IEnumerable<RateLimitConfigOptions> _optionsCache;
        public LocalRateLimitConfigProvider(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _rateLimitConfigSection = new Lazy<IConfiguration>(() =>
            {

                var configSection = configuration.GetSection("RateLimitConfigurations");
                if (configSection == null)
                {
                    throw new ArgumentException("could not find `RateLimitConfigurations` section in IConfiguration");
                }
                return configSection;
            });
        }

        public IEnumerable<RateLimitConfigOptions> GetConfig()
        {
            if (_optionsCache == null)
            {
                _optionsCache = _rateLimitConfigSection.Value.Get<IEnumerable<RateLimitConfigOptions>>();
                // empty, '/' paths are invalid.
                // TODO: we should have more checks for invalid config.
                var hasAllCatchPath = _optionsCache.Any(option => option.EndsWithPath.Equals("/") || option.EndsWithPath == string.Empty || string.IsNullOrWhiteSpace(option.EndsWithPath));
                if (hasAllCatchPath)
                {
                    throw new Exception("invalid EndsWithPath");
                }
            }
            return _optionsCache;
        }
    }
}