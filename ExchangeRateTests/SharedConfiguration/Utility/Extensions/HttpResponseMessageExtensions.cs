using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateTests.SharedConfiguration.Utility.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T?> ParseContent<T>(this HttpResponseMessage httpResponseMessage)
        {
            var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            if (contentString == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(contentString);
        }
    }
}
