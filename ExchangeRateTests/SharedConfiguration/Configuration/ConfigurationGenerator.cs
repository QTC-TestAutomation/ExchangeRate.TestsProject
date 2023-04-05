using BoDi;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.TestFramework;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework.Internal;
using Microsoft.Extensions;
using Microsoft.Extensions.Logging;
using ExchangeRateTests.SharedConfiguration.Utility.ApiCallers;
using ExchangeRateTests.SharedConfiguration.Utility.ApiClient;
using ExchangeRateTests.SharedConfiguration.Utility.Constants;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Configuration;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Interface;

namespace ExchangeRateTests.SharedConfiguration.Configuration
{
    public interface IConfigurationGenerator
    {
        public void BindApiCallers(ConfigurationHelper configurationHelper);
        public ConfigurationHelper BindConfig();
        public void CheckAndOverWriteEnvironmentVariables();
        public void SetInitialConfiguration(ConfigurationHelper configurationHelper);
    }

    [Binding]
    public class ConfigurationGenerator : IConfigurationGenerator
    {
        private readonly ITestRunContext _testRunContext;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly IObjectContainer _objectContainer;

        public ConfigurationGenerator(ITestRunContext testRunContext, ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer objectContainer)
        {
            _testRunContext = testRunContext;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _objectContainer = objectContainer;
        }

        public void CheckAndOverWriteEnvironmentVariables()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline)))
            {
                Environment.SetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline, EnvironmentVariableValues.Test);
            }
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment)))
            {
                Environment.SetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment, EnvironmentVariableValues.Test);
            }
        }

        public void SetInitialConfiguration(ConfigurationHelper configurationHelper)
        {

        }

        public ConfigurationHelper BindConfig()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment);

            string appsettingPath = environment == null ? $"appsettings.json" : $"appsettings.{environment}.json";
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<ConfigurationGenerator>(true)
                .SetBasePath(_testRunContext.GetTestDirectory())
                .AddJsonFile(appsettingPath, optional: true).Build();

            ConfigurationHelper configurationHelper = new(config);

            //var testpipeline = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline);

            //if (!string.IsNullOrEmpty(testpipeline) && !testpipeline.Equals(EnvironmentVariableValues.Local, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    configurationHelper.ExternalConnections.ConnectionString = $"{Environment.GetEnvironmentVariable("SQL_CONNECTIONSTRING")}InitialCatalog={configurationHelper.ExternalConnections.DatabaseName};";
            //}
            //else
            //{
            //    var connection = $"connectionString-{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment)}";
            //    configurationHelper.ExternalConnections.ConnectionString = $"{config.GetValue<string>(connection)}Initial Catalog={configurationHelper.ExternalConnections.DatabaseName};";
            //}


            _objectContainer.RegisterInstanceAs(config);
            _objectContainer.RegisterInstanceAs(configurationHelper);

            return configurationHelper;
        }

        public void BindApiCallers(ConfigurationHelper configurationHelper)
        {
            _objectContainer.RegisterTypeAs<ApiClient, IApiClient>().InstancePerContext();
            _objectContainer.RegisterInstanceAs(configurationHelper, typeof(IConfigurationHelper));
            _objectContainer.RegisterTypeAs<ConfigurationHelper, IConfigurationHelper>().InstancePerContext();

            //ExchangeRate
            _objectContainer.RegisterTypeAs<GetConvertApiCaller, IGetConvertApiCaller>().InstancePerContext();
        }
    }
}
