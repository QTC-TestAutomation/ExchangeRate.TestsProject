using ExchangeRateTests.SharedConfiguration.Utility.Extensions;
using ExchangeRateTests.SharedConfiguration.Utility.RequestBodies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateTests.SharedConfiguration.Utility.ApiClient
{
    public interface IApiClient
    {
        Task<T> Delete<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "");
        Task<T> Get<T>(string url, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "");
        Task<T> Patch<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "");
        Task<T> Post<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "");
        Task<T> Put<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "");
    }
    public class ApiClient : IApiClient
    {
        private HttpClient _httpClient = new(new HttpClientHandler());

        public ApiClient()
        {
            ConstructNewClient(30);
        }

        private async Task<T> Call<T>(string url, HttpMethod httpMethod, IRequestBody? requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            UpdateTimeOut(timeoutSeconds);
            AddAuthToken(authToken);
            HttpRequestMessage httpRequestMessage = new(httpMethod, url);
            httpRequestMessage = requestBody != null ? AddRequestContent(httpRequestMessage, requestBody, apiKeyHeader) : httpRequestMessage;

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (typeof(T) == typeof(HttpResponseMessage))
            {
                return (T)(object)responseMessage;
            }
            else if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.ParseContent<T>() ?? throw new Exception("ParseContent returned null.");
            }

            throw new Exception(await BuildErrorMessage(url, responseMessage, httpMethod.Method.ToUpper(), requestBody));
        }


        public async Task<T> Get<T>(string url, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            return await Call<T>(url, HttpMethod.Get, null, authToken, timeoutSeconds, apiKeyHeader);
        }
        public async Task<T> Post<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            return await Call<T>(url, HttpMethod.Post, requestBody, authToken, timeoutSeconds, apiKeyHeader);
        }
        public async Task<T> Put<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            return await Call<T>(url, HttpMethod.Put, requestBody, authToken, timeoutSeconds, apiKeyHeader);
        }
        public async Task<T> Patch<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            return await Call<T>(url, HttpMethod.Patch, requestBody, authToken, timeoutSeconds, apiKeyHeader);
        }

        public async Task<T> Delete<T>(string url, IRequestBody requestBody, string authToken = "", int timeoutSeconds = 30, string apiKeyHeader = "")
        {
            return await Call<T>(url, HttpMethod.Delete, requestBody, authToken, timeoutSeconds, apiKeyHeader);
        }

        private async Task<string> BuildErrorMessage(string url, HttpResponseMessage httpResponseMessage, string requestType, IRequestBody? requestBody = null)
        {
            string error = $"Error with {requestType} for Url {url}, \n Http Status Code: {httpResponseMessage.StatusCode}, ReasonPhrase: " +
                $"{httpResponseMessage.ReasonPhrase}, \nRequestMessage: {httpResponseMessage.RequestMessage}" +
                $"\nResponseContent: {await httpResponseMessage.Content.ReadAsStringAsync()}";

            if (requestBody != null)
            {
                error += $", \nRequestBody: {JsonConvert.SerializeObject(requestBody)}";
            }

            return error;
        }

        private void AddAuthToken(string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrEmpty(authToken) ? new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken) : null;
        }

        private HttpRequestMessage AddRequestContent(HttpRequestMessage httpRequestMessage, IRequestBody requestBody, string? apiKeyHeader = null, string? apiHost = null)
        {
            if (requestBody != null)
            {
                var body = JsonConvert.SerializeObject(requestBody);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(apiKeyHeader))
            {
                httpRequestMessage.Headers.Add("apiHost", apiHost);
                httpRequestMessage.Headers.Add("apiKey", apiKeyHeader);
                httpRequestMessage.Headers.Add("Cache-Control", "no-cache");
                httpRequestMessage.Headers.Add("Accept-Encoding", "gzip, defalte, br");
                httpRequestMessage.Headers.Add("Accept", "*/*");
            }

            return httpRequestMessage;
        }

        private void UpdateTimeOut(int timeOutSeconds)
        {
            if (_httpClient.Timeout.Seconds != timeOutSeconds)
            {
                ConstructNewClient(timeOutSeconds);
            }
        }

        private void ConstructNewClient(int timeoutSeconds)
        {
            _httpClient = new HttpClient(new HttpClientHandler())
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }
    }
}
