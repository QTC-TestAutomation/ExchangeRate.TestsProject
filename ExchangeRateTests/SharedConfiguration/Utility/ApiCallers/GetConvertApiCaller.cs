using ExchangeRateTests.SharedConfiguration.Utility.ApiClient;
using ExchangeRateTests.SharedConfiguration.Utility.Constants;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Interface;
using ExchangeRateTests.SharedConfiguration.Utility.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateTests.SharedConfiguration.Utility.ApiCallers
{
    public interface IGetConvertApiCaller
    {
        public Task<HttpResponseMessage> GetConvertResponse(string? fromCurrency = null, string? toCurrency = null, decimal? amount = null);
        public Task<ConvertRoot> GetConvert(string? fromCurrency = null, string? toCurrency = null, decimal? amount = null);
    }
    public class GetConvertApiCaller : IGetConvertApiCaller
    {
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IApiClient _apiClient;

        public GetConvertApiCaller(IConfigurationHelper configurationHelper, IApiClient apiClient)
        {
            _configurationHelper = configurationHelper;
            _apiClient = apiClient;
        }

        public async Task<HttpResponseMessage> GetConvertResponse(string? fromCurrency = null, string? toCurrency = null, decimal? amount = null)
        {
            string url = _configurationHelper.GetExchangeRateBaseApiUrl() + ExchangeRateApiEndpoints.Convert;
            if (!string.IsNullOrEmpty(fromCurrency))
            {
                url += $"?from={fromCurrency}";
                if (!string.IsNullOrEmpty(toCurrency))
                {
                    url += $"&to={toCurrency}";
                }
                if (amount != null)
                {
                    url += $"&amount={amount}";
                }
            }
            else if (!string.IsNullOrEmpty(toCurrency))
            {
                url += $"?to={toCurrency}";
                if (amount != null)
                {
                    url += $"&amount={amount}";
                }
            }
            return await _apiClient.Get<HttpResponseMessage>(url, "", 30);
        }

        public async Task<ConvertRoot> GetConvert(string? fromCurrency = null, string? toCurrency = null, decimal? amount = null)
        {
            string url = _configurationHelper.GetExchangeRateBaseApiUrl() + ExchangeRateApiEndpoints.Convert;
            if (!string.IsNullOrEmpty(fromCurrency))
            {
                url += $"?from={fromCurrency}";
                if (!string.IsNullOrEmpty(toCurrency))
                {
                    url += $"&to={toCurrency}";
                }
                if (amount != null)
                {
                    url += $"&amount={amount}";
                }
            }
            else if (!string.IsNullOrEmpty(toCurrency))
            {
                url += $"?to={toCurrency}";
                if (amount != null)
                {
                    url += $"&amount={amount}";
                }
            }
            return await _apiClient.Get<ConvertRoot>(url, "", 30);
        }
    }
}
