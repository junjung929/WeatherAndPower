using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WeatherAndPower.Data
{
    public class Fingrid : BaseHttpClient
    {
        private static string SERVER_URL = "https://api.fingrid.fi/v1/variable";
        private static string FINGRID_API = GetApiKey();
        private static string DEFAULT_FORMAT = "xml";

        /// <summary>
        /// Get Fingrid api key from environment variables
        /// </summary>
        /// <returns>Fingrid api key</returns>
        private static string GetApiKey()
        {
            string apikey = Environment.GetEnvironmentVariable("fingrid_api");
            if (apikey == null)
            {
                throw new ArgumentNullException("Fingrid API Key cannot be found");
            }
            return apikey;
        }

        public static void Get(UInt16 variableId, DateTime startTime, DateTime endTime)
        {
            Get(variableId, startTime, endTime, DEFAULT_FORMAT);
        }

        public static async void Get(UInt16 variableId, DateTime startTime, DateTime endTime, string format)
        {
            string query = ParseParamsToQuery(startTime, endTime);
            string requestUri = ParseRequestUri(variableId, query, format);
            string contentType;

            if (format == "csv")
            {
                contentType = $"text/{format}";
            }
            else
            {
                contentType = $"application/{format}";
            }

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "x-api-key", FINGRID_API },
                    { "Accept", contentType }
                }
            };

            var response = await _client.SendAsync(httpRequestMessage);
            Console.WriteLine(response);


        }

        /// <summary>
        /// Generate request URI based on the given parameters
        /// </summary>
        /// <param name="variableId">ID of information to be retrieved </param>
        /// <param name="query"></param>
        /// <param name="format">format for return data csv | json | xml</param>
        /// <returns>Request URI</returns>
        private static string ParseRequestUri(UInt16 variableId, string query, string format)
        {

            string request = $"{SERVER_URL}/{variableId}/events/{format}?{query}";

            return request;
        }

        /// <summary>
        /// Parse the given datetime to fit the format for the API parameter
        /// </summary>
        /// <param name="dateTime">DateTime parameter to parse</param>
        /// <returns>
        /// Correctly formatted datetime variable
        /// </returns>
        private static string ParseDateTimeFormat(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddThh:mm:ssZ");
        }

        /// <summary>
        /// Parse parameters to HTTP query string
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>query string</returns>
        private static string ParseParamsToQuery(DateTime startTime, DateTime endTime)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("start_time", ParseDateTimeFormat(startTime));
            requestParams.Add("end_time", ParseDateTimeFormat(endTime));

            return string.Join("&", requestParams.ToList().Select(x =>
             x.Key + "=" + x.Value
            ));
        }

    }
}
