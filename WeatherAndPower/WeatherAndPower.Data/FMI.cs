using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Data
{
    public class FMI : BaseHttpClient
    {

        public static string Place { get; set; }
        public static string Query { get; set; }
        public static string Parameters { get; set; }
        public static string StartTime { get; set; }
        public static string EndTime { get; set; }




        // Useful parameter explanations
        Dictionary<string, string> OBSERVATION_PARAMS = new Dictionary<string, string>()
        {
            {"t2m","Air Temperature"},// Celsius		"temperature" alternative
			{"ws_10min","Wind speed"},// m/s
			{"rh", "Relative humidity"}, // %
			{"r_1h","Precipitation amount"}, // mm
			{"n_man", "Cloud amount" }, //  1/8
			{"wawa", "Present weather (auto)"} // I dunno what that means
		};

        Dictionary<string, string> FORECAST_PARAMS = new Dictionary<string, string>()
        {
            {"Temperature", "Air Temperature" }, // Celsius
			{"WindSpeedMS", "Wind speed" }, // m/s 
			{"Humidity", "Relative humidity"}, // %
			{"Precipitation1h", "Hourly precipitation"}, // mm
			{"PrecipitationAmount", "Precipitation Amount" }, // mm
			{ "TotalCloudCover", "Cloudiness"}, // %			
		};
        Dictionary<string, string> MEDIAN_PARAMS = new Dictionary<string, string>()
        {
            {"TA_PT1H_AVG", "Average temperature"},
            { "TA_PT1H_MAX", "Max temperature"},
            { "TA_PT1H_MIN","Min temperature" }
        };

        public static readonly Dictionary<string, DataFormat> FORMAT_PARAMS = new Dictionary<string, DataFormat>()
        {
            {"t2m", DataFormat.Temperature},
            {"Temperature", DataFormat.Temperature},
            {"TA_PT1H_AVG", DataFormat.Temperature},
            {"TA_PT1H_MAX", DataFormat.Temperature},
            {"TA_PT1H_MIN", DataFormat.Temperature},
            {"ws_10min", DataFormat.Wind},
            {"WindSpeedMS", DataFormat.Wind },
            {"rh", DataFormat.Humidity},
            {"Humidity", DataFormat.Humidity},
            {"r_1h", DataFormat.Precipitation},
            {"PrecipitationAmount", DataFormat.Precipitation},
            {"n_man",DataFormat.Cloudiness},
            {"TotalCloudCover",DataFormat.Cloudiness},
        };


        static string SERVER_URL = "http://opendata.fmi.fi/wfs?service=WFS&version=2.0.0&request=getFeature&";

        // MINIMUM REQUIRMENTS QUERIES
        //	fmi::observations::weather::simple	
        //	fmi::observations::weather::hourly::simple			AVERAGE comes from hourly
        //	fmi::forecast::hirlam::surface::point::simple

        /// <summary>
        /// Looks pretty bad, sorry
        /// </summary>
        /// <param name="mode"> 
        ///		either observations::weather
        ///		or forecast::hirlam::surface::point
        /// </param>
        /// <param name="format">
        ///		simple
        ///		timevaluepair
        ///		multipointcoverage
        /// </param>
        /// <returns></returns>
        public static string BuildQuery(string mode)
        {
            string query = "fmi";
            string separator = "::";
            string format = "timevaluepair";
            query += separator + mode + separator + format;
            return query;
        }


        public static string BuildRequest(string query)
        {
            var options = new Dictionary<string, string>();
            // Default timestep 60 minutes
            options.Add("timestep", "60");

            options.Add("storedquery_id", query);
            options.Add("place", Place);
            options.Add("parameters", Parameters);
            options.Add("starttime", StartTime);
            options.Add("endtime", EndTime);


            string request = $"{SERVER_URL}" + string.Join("&", options.ToList().Select(x =>
              x.Key + "=" + x.Value));
            return request;
        }

        public static async Task<List<DataSeries>> GetData(string url)
        {
            var httpResponse = await _client.GetAsync(url);
            var bytes = await httpResponse.Content.ReadAsByteArrayAsync();
            var body = System.Text.Encoding.Default.GetString(bytes);

            var doc = new XmlDocument();
            doc.LoadXml(body);


            XmlNamespaceManager mng = CreateManager(doc);

            //	Handling erroneous parameters
            XmlElement Root = doc.DocumentElement;
            if (Root.Name == "ExceptionReport")
            {
                XmlNode ExcTextNode = Root.FirstChild.FirstChild;
                if (ExcTextNode != null)
                {
                    string ExceptionMessage = ExcTextNode.InnerText;
                    throw new Exception(ExceptionMessage);
                }
            }

            //	Handling missing data parameters
            else if (Root.Attributes != null && Root.Attributes["numberMatched"] != null)
            {
                string num_of_datasets = Root.Attributes["numberMatched"].Value;
                if (num_of_datasets == "0")
                {
                    throw new Exception("Data unavailable");
                }
            }

            var result_nodes = doc.SelectNodes("//om:result", mng);
            List<DataSeries> plots = new List<DataSeries>();

            foreach (XmlNode result in result_nodes)
            {

                List<Tuple<DateTime, IData>> series = new List<Tuple<DateTime, IData>>();
                DataFormat format = SetFormat(result, mng);
                var plot = new DataSeries(Place, format, series);

                // VALUETIMEPAIR				
                var PairLst = result.SelectNodes(".//wml2:MeasurementTVP", mng);
                foreach (XmlNode TimeValuePair in PairLst)
                {
                    DateTime time = Convert.ToDateTime(TimeValuePair.SelectSingleNode(".//wml2:time", mng).InnerText);
                    double.TryParse(TimeValuePair.SelectSingleNode(".//wml2:value", mng).InnerText, out double value);

                    if (Double.IsNaN(value))
                    {
                        throw new Exception("Data unavailable");
                    }

                    Temperature data = new Temperature(value);
                    Tuple<DateTime, IData> plotPoint = new Tuple<DateTime, IData>(time, data);
                    series.Add(plotPoint);

                    Console.WriteLine(time);
                    Console.WriteLine(value);
                }

                plots.Add(plot);
            }
            return plots;
        }

        private static XmlNamespaceManager CreateManager(XmlDocument doc)
        {
            var mng = new XmlNamespaceManager(doc.NameTable);

            mng.AddNamespace("om", "http://www.opengis.net/om/2.0");
            mng.AddNamespace("wml2", "http://www.opengis.net/waterml/2.0");
            mng.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mng.AddNamespace("gml", "http://www.opengis.net/gml/3.2");

            return mng;
        }
        /// <summary>
        /// Sets the data format based on the given parameter
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mng"></param>
        /// <returns></returns>
        private static DataFormat SetFormat(XmlNode node, XmlNamespaceManager mng)
        {
            var param_id = node.SelectSingleNode(".//wml2:MeasurementTimeseries", mng);
            string attribute = param_id.Attributes["gml:id"].Value;
            string parameter = attribute.Split('-').Last();
            DataFormat format = FORMAT_PARAMS[parameter];
            Console.WriteLine(format);
            return format;
        }
    }
}

