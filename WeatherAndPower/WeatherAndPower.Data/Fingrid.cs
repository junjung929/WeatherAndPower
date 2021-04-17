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

        public static IDataSeriesFactory DataSeriesFactory { get; set; }

        public static Dictionary<string, string> contentTypes = new Dictionary<string, string>()
        {
            { "csv","text/csv" },
            { "xml", "application/xml" },
            { "json", "application/json" }
        };


        /**
         * Get Fingrid api key from environment variables
         */
        private static string GetApiKey()
        {
            string apikey = Environment.GetEnvironmentVariable("fingrid_api");
            if (apikey == null)
            {
                throw new ArgumentNullException("Fingrid API Key cannot be found");
            }
            return apikey;
        }

        /**
         * Get a single and latest data point from Fingrid API
         */
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

        /**
         * Get request to retrieve power information with the given PowerType and parameters
         */
        public static async Task<IDataSeries> Get(PowerType powerType, DateTime startTime, DateTime endTime, string format = null)
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
                IDataSeries dataseries = ParseXMLToDataSeries(powerType.Id, body);
                dataseries.IsComparable = powerType.Source != PowerType.SourceEnum.All;
                return dataseries;
            }
            else
            {
                Console.WriteLine($"Return data type {format} doesn't support yet");
                throw new NotImplementedException($"Return data type {format} doesn't support yet");
            }

        }

        /*
         * Set Api key and content type into headers.
         */
        private static void SetHeaders(HttpRequestMessage httpRequestMessage, string format)
        {
            httpRequestMessage.Headers.Add("x-api-key", FINGRID_API);
            httpRequestMessage.Headers.Add("Accept", contentTypes[format]);
        }

        /*
         * Generate request URI based on the given parameters
         */
        private static string ParseRequestUri(int variableId, string query, string format)
        {
            string request = $"{SERVER_URL}/{variableId}/events/{format}?{query}";
            return request;
        }

        /*
         * Parse the given datetime to fit the format for the API parameter
         */
        private static string ParseDateTimeFormat(DateTime dateTime)
        {
            return dateTime.ToString(DATETIME_FORMAT);
        }

        /*
         * Parse parameters to HTTP query string
         */
        private static string ParseParamsToQuery(DateTime startTime, DateTime endTime)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("start_time", ParseDateTimeFormat(startTime));
            requestParams.Add("end_time", ParseDateTimeFormat(endTime));

            return string.Join("&", requestParams.ToList().Select(x =>
             x.Key + "=" + x.Value
            ));
        }

        /*
         * Parse XML string to a single data
         */
        private static IData ParseXMLToSingleData(int variableId, string XMLString)
        {
            var doc = new XmlDocument();
            doc.LoadXml(XMLString);

            var evt = doc.SelectSingleNode("event");
            string val = evt.SelectSingleNode("value").InnerText;
            string start_time = evt.SelectSingleNode("start_time").InnerText;
            string end_time = evt.SelectSingleNode("end_time").InnerText;
            IData power = new Power(Double.Parse(val));
            return power;
        }

        /*
         * Parse XML string to DataSeries
         */
        private static IDataSeries ParseXMLToDataSeries(int variableId, string XMLString)
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
                    //Power data = new Power(variableId, val, startTime, endTime); // IData objects should only contain value
                    Power data = new Power(val);
                    Tuple<DateTime, IData> plotPoint = new Tuple<DateTime, IData>(startTime, data);
                    powerSeries.Add(plotPoint);
                }
            }
            return DataSeriesFactory.CreateDataSeries("", DataFormat.Power, powerSeries);
        }
    }
}
