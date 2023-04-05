using BoDi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.TestFramework;
using ExchangeRateTests.SharedConfiguration.Configuration;

namespace ExchangeRateTests.EndpointTests.Hooks
{
    [Binding]
    public class BeforeBindingHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly ITestRunContext _testRunContext;
        private readonly IConfigurationGenerator _configurationGenerator;

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public BeforeBindingHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext, ITestRunContext testRunContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _testRunContext = testRunContext;
            _configurationGenerator = new ConfigurationGenerator(_testRunContext, _scenarioContext, _featureContext, _objectContainer);
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }



        [BeforeScenario]
        public void BeforeScenario()
        {
            _configurationGenerator.CheckAndOverWriteEnvironmentVariables();
            var configurationHelper = _configurationGenerator.BindConfig();
            _configurationGenerator.SetInitialConfiguration(configurationHelper);
            _configurationGenerator.BindApiCallers(configurationHelper);
        }


        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }

    }
}
