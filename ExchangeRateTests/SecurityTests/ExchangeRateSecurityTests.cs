using ExchangeRateTests.SharedConfiguration.Utility.ApiClient;
using ExchangeRateTests.SharedConfiguration.Utility.Constants;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Configuration;
using ExchangeRateTests.SharedConfiguration.Utility.Helpers.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;

class ExchangeRateSecurityTests
{
    private readonly IConfigurationHelper _configurationHelper;
    private readonly IApiClient _apiClient;

    public ExchangeRateSecurityTests(IConfigurationHelper configurationHelper, IApiClient apiClient)
    {
        _configurationHelper = configurationHelper;
        _apiClient = apiClient;
    }
    static void Main(string[] args)
    {
         var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
        .Build();
        var configurationHelper = new ConfigurationHelper(config);
        // SSL/TLS certificate validation
        ServicePointManager.ServerCertificateValidationCallback = ValidateCertificate;

        // Input validation
        Console.WriteLine("Please enter an input for the from Currency: ");
        string input = args[0];

        if (input != null && Regex.IsMatch(input, @"^[a-zA-Z]{3}$"))
        {
            Console.WriteLine("Input is safe to use.");
        }
        else
        {
            Console.WriteLine("Input is not safe to use. It should be a valid currency code.");
        }

        // Cross-Site Scripting (XSS) prevention
        string xssInput = "<script>alert('XSS attack');</script>";
        string encodedInput = HttpUtility.HtmlEncode(xssInput);
        Console.WriteLine("Encoded input: " + encodedInput);

        // API request
        HttpClient client = new();
        string url = configurationHelper.GetExchangeRateBaseApiUrl() + ExchangeRateApiEndpoints.Convert;
        HttpResponseMessage response = client.GetAsync(url + "?from=" + input + "&to=eur").Result;
        string responseString = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(responseString);
    }

    // Validate the SSL/TLS certificate
    static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // Add logic to validate the certificate here
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;
        else
            return false;
    }
}
