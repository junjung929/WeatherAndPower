using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Data
{
    public class Fingrid : BaseHttpClient
    {
        private static string SERVER_URL = "https://api.fingrid.fi/v1/variable";
        private static string FINGRID_API = GetApiKey();
        private static string DEFAULT_FORMAT = "xml";

        public static Dictionary<string, string> contentTypes = new Dictionary<string, string>()
        {
            { "csv","text/csv" },
            { "xml", "application/xml" },
            { "json", "application/json" }
        };
        public static Dictionary<UInt16, string> powerTypes = new Dictionary<ushort, string>()
        {
            { 124, "Electricity consumption in Finland" },
            { 165, "Electricity consumption forecast for the next 24 hours" },
            { 242, "A tentative production prediction for the next 24 hours as hourly energy" },
            { 74, "Electricity production in Finland" },
            { 75, "Wind power generation - hourly data" },
            { 181, "Wind power production - real time data" },
            { 188, "Nuclear power production - real time data" },
            { 191, "Hydro power production - real time data" },
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

        // Get request to retrieve power information with default return format "xml"
        // TODO: return value to Contracts
        public static async void Get(UInt16 variableId, DateTime startTime, DateTime endTime)
        {
            Get(variableId, startTime, endTime, DEFAULT_FORMAT);
        }

        /// <summary>
        /// Get request to retrieve power information with the given id and parameters
        /// </summary>
        /// <param name="variableId">ID of power information type</param>
        /// <param name="startTime">Beginning of date time range</param>
        /// <param name="endTime">Ending of date time range</param>
        /// <param name="format">Return data format csv | json | xml</param>
        /// // TODO: return value to Contracts
        public static async void Get(UInt16 variableId, DateTime startTime, DateTime endTime, string format)
        {
            string query = ParseParamsToQuery(startTime, endTime);
            string requestUri = ParseRequestUri(variableId, query, format);


            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(requestUri);
            setHeaders(httpRequestMessage, format);

            var response = await _client.SendAsync(httpRequestMessage);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            if (format == "xml")
            {
                DataSeries dataseries = ParseXMLToObject(variableId, body);
                Console.WriteLine("Printing result " + dataseries.Name);
                foreach (var data in dataseries.Series)
                {
                    Console.WriteLine(data.Item1.ToString("yyyy-MM-ddThh:mm:ssZ") + ", " + data.Item2.Value);
                }
            }
            else
            {
                Console.WriteLine($"Return data type {format} doesn't support yet");
            }

        }

        /// <summary>
        /// Set Api key and content type into headers.
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <param name="format"></param>
        private static void setHeaders(HttpRequestMessage httpRequestMessage, string format)
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

        /// <summary>
        /// Parse XML string to object
        /// </summary>
        /// <param name="XMLString"></param>
        /// <returns>DataSeries with power type and series of information</returns>
        private static DataSeries ParseXMLToObject(UInt16 variableId,string XMLString)
        {
            List<Tuple<DateTime, IData>> powerSeries = new List<Tuple<DateTime, IData>>();
            var doc = new XmlDocument();
            doc.LoadXml(XMLString);

            XmlNodeList evts = doc.SelectNodes("events/event");
            Console.WriteLine(evts.Count);

            foreach (XmlNode evt in evts)
            {
                if (evt != null)
                {
                    string val = evt.SelectSingleNode("value").InnerText;
                    string start_time = evt.SelectSingleNode("start_time").InnerText;
                    string end_time = evt.SelectSingleNode("end_time").InnerText;
                    IData power = new Power(Double.Parse(val), DateTime.Parse(start_time), DateTime.Parse(end_time));
                    powerSeries.Add(Tuple.Create(DateTime.Parse(start_time), power));
                    Console.WriteLine($"{start_time} {end_time} {val}");
                }
            }
            return new DataSeries(powerTypes[variableId], powerSeries);
        }
    }
}
