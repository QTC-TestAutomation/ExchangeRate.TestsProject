using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using NUnit.Framework;
using ExchangeRateTests.SharedConfiguration.Utility.ApiCallers;
using ExchangeRateTests.SharedConfiguration.Utility.Models;

namespace ExchangeRateTests.EndpointTests.StepDefinitions
{
    [Binding]
    public sealed class ExchangeRateStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private GetConvertApiCaller _getConvertApiCaller;
        private HttpResponseMessage? _httpResponseMessage;
        private string? _fromCurrency;
        private string? _toCurrency;
        private decimal? _amount;

        public ExchangeRateStepDefinitions(GetConvertApiCaller getConvertApiCaller)
        {
            _getConvertApiCaller = getConvertApiCaller;
        }

        [Given(@"I want to convert (.*) to (.*)")]
        public void GivenIWantToConvert(string fromCurrency, string toCurrency)
        {
            _fromCurrency = fromCurrency;
            _toCurrency = toCurrency;
        }

        [When(@"I call the public exchange rate conversion API with (.*) and (.*) currencies")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithFromAndToCurrencies(string fromCurrency, string toCurrency)
        {
            if (_getConvertApiCaller != null)
            {
                _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(fromCurrency, toCurrency);
            }
        }

        [Then(@"the API should return a success response with (.*) exchange rate")]
        public async Task ThenTheAPIShouldReturnASuccessResponse(string exchangeRate)
        {
            var response = _httpResponseMessage;

            if (response != null)
            {
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var responseContent = await response.Content.ReadAsStringAsync();

                var convertResult = JsonConvert.DeserializeObject<ConvertRoot>(responseContent);

                convertResult.Should().NotBeNull();
                if (convertResult != null)
                {
                    if (convertResult.Motd != null)
                    {
                        convertResult.Motd.Msg.Should().Be("If you or your company use this project or like what we doing, please consider backing us so we can continue maintaining and evolving this project.");
                        convertResult.Motd.Url.Should().Be("https://exchangerate.host/#/donate");
                    }
                    convertResult.Success.Should().BeTrue();
                    if (convertResult.Query != null)
                    {
                        if (_fromCurrency != null)
                        {
                            convertResult.Query.From.Should().Be(_fromCurrency.ToUpper());
                        }
                        if (_toCurrency != null)
                        {
                            convertResult.Query.To.Should().Be(_toCurrency.ToUpper());
                        }
                        if (_amount != null)
                        {
                            convertResult.Query.Amount.Should().Be(1);
                        }
                    }
                    if (convertResult.Info != null)
                    {
                        if (exchangeRate == "valid")
                        {
                            convertResult.Info.Rate.Should().NotBe(0);
                            convertResult.Result.Should().NotBe(0);
                        }
                        else if (exchangeRate == null)
                        {
                            convertResult.Info.Rate.Should().Be(null);
                            convertResult.Result.Should().Be(null);
                        }
                        else if (exchangeRate == "1")
                        {
                            convertResult.Info.Rate.Should().Be(1);
                            convertResult.Result.Should().Be(1);
                        }
                    }
                    convertResult.Historical.Should().BeFalse();
                    convertResult.Date.Should().Be(DateTime.UtcNow.Date);
                }

            }
            else
            {
                Assert.Fail("Response is null.");
            }
        }

        [Given(@"I want to convert to an invalid currency code")]
        public void GivenIWantToConvertUSDToAnInvalidCurrencyCode()
        {
            _fromCurrency = "USD";
            _toCurrency = "invalid";
        }

        [When(@"I call the public exchange rate conversion API with USD and an invalid currency code")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithUSDAndAnInvalidCurrencyCode()
        {
            if (_getConvertApiCaller != null)
            {
                _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(_fromCurrency, _toCurrency);
            }
        }


        [Given(@"I want to convert with a missing source currency code")]
        public void GivenIWantToConvertWithAMissingSourceCurrencyCode()
        {
            _toCurrency = "EUR";
        }

        [When(@"I call the public exchange rate conversion API with a missing source currency code and EUR")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithAMissingSourceCurrencyCodeAndEUR()
        {
            _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(null, _toCurrency);
        }

        [Given(@"I want to convert with a missing target currency code")]
        public void GivenIWantToConvertWithAMissingTargetCurrencyCode()
        {
            _fromCurrency = "USD";
        }

        [When(@"I call the public exchange rate conversion API with USD and a missing target currency code")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithUSDAndAMissingTargetCurrencyCode()
        {
            if (_getConvertApiCaller != null)
            {
                _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(_fromCurrency);
            }
        }

        [Given(@"I want to convert USD with an invalid amount")]
        public void GivenIWantToConvertUSDWithAnInvalidAmount()
        {
            _fromCurrency = "USD";
            if (decimal.TryParse("amount", out decimal result))
            {
                _amount = result;
            }
        }

        [When(@"I call the public exchange rate conversion API with an invalid amount and EUR")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithAnInvalidAmountAndEUR()
        {
            _toCurrency = "eur";
            if (_getConvertApiCaller != null)
            {
                _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(_fromCurrency, _toCurrency, _amount);
            }
        }

        [Given(@"I want to convert USD with a negative amount")]
        public void GivenIWantToConvertUSDWithANegativeAmount()
        {
            _fromCurrency = "USD";
            _amount = -1;
        }

        [When(@"I call the public exchange rate conversion API with a negative amount and EUR")]
        public async Task WhenICallThePublicExchangeRateConversionAPIWithANegativeAmountAndEUR()
        {
            _toCurrency = "eur";
            if (_getConvertApiCaller != null)
            {
                _httpResponseMessage = await _getConvertApiCaller.GetConvertResponse(_fromCurrency, _toCurrency, _amount);
            }
        }
    }
}