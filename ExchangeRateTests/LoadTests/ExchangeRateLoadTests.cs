using NBomber.CSharp;
using NBomber.Contracts;
using TechTalk.SpecFlow;
using NBomber.Http.CSharp;
using Spectre.Console;
using NBomber.Contracts.Stats;
using System.Net.Http;
using System.Threading;
using ExchangeRateTests.SharedConfiguration.Configuration;
using ExchangeRateTests.SharedConfiguration.Utility.Constants;
using ExchangeRateTests.SharedConfiguration.Utility.ApiClient;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Interface;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Configuration;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateLoadTests
{
    class Program
    {
        static void Main(string[] args)
        {
             var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
            .Build();
            var configurationHelper = new ConfigurationHelper(config);
            var apiClient = new ApiClient();
            var exchangeRateLoadTests = new ExchangeRateLoadTests(configurationHelper, apiClient);
            var cancellationTokenSource = new CancellationTokenSource();
            exchangeRateLoadTests.RunLoadTestsAsync();
            Thread.Sleep(TimeSpan.FromSeconds(60)); // run for 60 seconds
            cancellationTokenSource.Cancel();
        }
    }

    public class ExchangeRateLoadTests
    {
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IApiClient _apiClient;

        public ExchangeRateLoadTests(IConfigurationHelper configurationHelper, IApiClient apiClient)
        {
            _configurationHelper = configurationHelper;
            _apiClient = apiClient;
        }
        public void RunLoadTestsAsync()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario
                .Create("exchange_rate_scenario", async context =>
                {
                    string url = _configurationHelper.GetExchangeRateBaseApiUrl() + ExchangeRateApiEndpoints.Convert + "?from=USD&to=eur";
                    var response = await httpClient.GetAsync(url);

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                })
                .WithLoadSimulations(
                    Simulation.Inject(rate: 10,
                        interval: TimeSpan.FromSeconds(1),
                        during: TimeSpan.FromSeconds(30)))

                .WithLoadSimulations(
                    Simulation.RampingInject(rate: 200,
                        interval: TimeSpan.FromSeconds(1),
                        during: TimeSpan.FromMinutes(1)),

                    Simulation.Inject(rate: 200,
                        interval: TimeSpan.FromSeconds(1),
                        during: TimeSpan.FromSeconds(30))

                );

            var testRunner = NBomberRunner
                .RegisterScenarios(scenario)
                .Run("test");
        }
    }
}
