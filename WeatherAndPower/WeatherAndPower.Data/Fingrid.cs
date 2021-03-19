using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using WeatherAndPower.Contracts;
using System.Globalization;

namespace WeatherAndPower.Data
{
    public class Fingrid : BaseHttpClient
    {
        private static string SERVER_URL = "https://api.fingrid.fi/v1/variable";
        private static string FINGRID_API = GetApiKey();
        private static string DEFAULT_FORMAT = "xml";
        private static string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";

        public static Dictionary<string, string> contentTypes = new Dictionary<string, string>()
        {
            { "csv","text/csv" },
            { "xml", "application/xml" },
            { "json", "application/json" }
        };


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

        public static async Task<IData> Get(PowerType powerType, string format = null)
        {
            if (format == null)
            {
                format = DEFAULT_FORMAT;
            }
            string requestUri = $"{SERVER_URL}/{powerType.Id}/event/{format}";

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(requestUri);
            SetHeaders(httpRequestMessage, format);

            HttpResponseMessage response;

            try
            {
                response = await _client.SendAsync(httpRequestMessage);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
           
            var body = await response.Content.ReadAsStringAsync();

            if (format == "xml")
            {
                IData singleData = ParseXMLToSingleData(powerType.Id, body);
                return singleData;
            }
            else
            {
                Console.WriteLine($"Return data type {format} doesn't support yet");
                throw new NotImplementedException($"Return data type {format} doesn't support yet");
            }

        }

        /// <summary>
        /// Get request to retrieve power information with the given id and parameters
        /// </summary>
        /// <param name="powerType">ID of power information type</param>
        /// <param name="startTime">Beginning of date time range</param>
        /// <param name="endTime">Ending of date time range</param>
        /// <param name="format">Return data format csv | json | xml</param>
        /// // TODO: return value to Contracts
        public static async Task<DataSeries> Get(PowerType powerType, DateTime startTime, DateTime endTime, string format = null)
        {
            if (format == null)
            {
                format = DEFAULT_FORMAT;
            }
            string query = ParseParamsToQuery(startTime, endTime);
            string requestUri = ParseRequestUri(powerType.Id, query, format);


            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(requestUri);
            SetHeaders(httpRequestMessage, format);

            HttpResponseMessage response;

            try
            {
                response = await _client.SendAsync(httpRequestMessage);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }

            var body = await response.Content.ReadAsStringAsync();

            if (format == "xml")
            {
                DataSeries dataseries = ParseXMLToDataSeries(powerType.Id, body);
                return dataseries;
            }
            else
            {
                Console.WriteLine($"Return data type {format} doesn't support yet");
                throw new NotImplementedException($"Return data type {format} doesn't support yet");
            }

        }

        /// <summary>
        /// Set Api key and content type into headers.
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <param name="format"></param>
        private static void SetHeaders(HttpRequestMessage httpRequestMessage, string format)
        {
            httpRequestMessage.Headers.Add("x-api-key", FINGRID_API);
            httpRequestMessage.Headers.Add("Accept", contentTypes[format]);
        }

        /// <summary>
        /// Generate request URI based on the given parameters
        /// </summary>
        /// <param name="variableId">ID of information to be retrieved </param>
        /// <param name="query"></param>
        /// <param name="format">format for return data csv | json | xml</param>
        /// <returns>Request URI</returns>
        private static string ParseRequestUri(int variableId, string query, string format)
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
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
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

        private static IData ParseXMLToSingleData(int variableId, string XMLString)
        {
            var doc = new XmlDocument();
            doc.LoadXml(XMLString);

            var evt = doc.SelectSingleNode("event");
            string val = evt.SelectSingleNode("value").InnerText;
            string start_time = evt.SelectSingleNode("start_time").InnerText;
            string end_time = evt.SelectSingleNode("end_time").InnerText;
            IData power = new Power(variableId, Double.Parse(val), DateTime.Parse(start_time), DateTime.Parse(end_time));
            return power;
        }

        /// <summary>
        /// Parse XML string to object
        /// </summary>
        /// <param name="XMLString"></param>
        /// <returns>DataSeries with power type and series of information</returns>
        private static DataSeries ParseXMLToDataSeries(int variableId, string XMLString)
        {
            List<Tuple<DateTime, IData>> powerSeries = new List<Tuple<DateTime, IData>>();
            var doc = new XmlDocument();
            doc.LoadXml(XMLString);

            XmlNodeList evts = doc.SelectNodes("events/event");

            foreach (XmlNode evt in evts)
            {
                if (evt != null)
                {
                    double val = double.Parse(evt.SelectSingleNode("value").InnerText, CultureInfo.InvariantCulture);
                    DateTime startTime = DateTime.Parse(evt.SelectSingleNode("start_time").InnerText);
                    DateTime endTime = DateTime.Parse(evt.SelectSingleNode("end_time").InnerText);
                    Power data = new Power(variableId, val, startTime, endTime);
                    Tuple<DateTime, IData> plotPoint = new Tuple<DateTime, IData>(startTime, data);
                    powerSeries.Add(plotPoint);
                }
            }
            return new DataSeries("", DataFormat.Power, powerSeries);
        }
    }
}
