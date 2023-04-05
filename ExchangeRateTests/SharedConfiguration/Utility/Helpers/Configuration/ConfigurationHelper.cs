using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ExchangeRateTests.SharedConfiguration.Utility.Helpers.Configuration
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public ExternalConnections? ExternalConnections { get; }

        public ConfigurationHelper(IConfiguration config)
        {
            ExternalConnections = config.GetSection(nameof(ExternalConnections)).Get<ExternalConnections>();
        }

        public string GetExchangeRateBaseApiUrl()
        {
            return ExternalConnections?.ExchangeRateBaseApiUrl ?? string.Empty;
        }
    }
}
