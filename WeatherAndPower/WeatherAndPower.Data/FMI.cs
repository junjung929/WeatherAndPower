﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeatherAndPower.Contracts;
using System.Windows.Forms;
using static WeatherAndPower.Contracts.Globals;
using System.Globalization;

namespace WeatherAndPower.Data
{
	public class FMI : BaseHttpClient
	{

		public static string Place { get; set; }
		public static string Query { get; set; }
		public static string Parameters { get; set; }
		public static string StartTime { get; set; }
		public static string EndTime { get; set; }
		public static string Timestep { get; set; }
		public static IDataSeriesFactory DataSeriesFactory { get; set; }

		// Useful parameter explanations
		private static readonly Dictionary<string, string> PARAMETERS = new Dictionary<string, string>()
		{
			{"t2m","Air Temperature"},// Celsius
			{"ws_10min","Wind speed"},// m/s
			{"rh", "Relative humidity"}, // %
			{"r_1h","Precipitation amount"}, // mm
			{"n_man", "Cloud amount" }, //  1/8
			{"Temperature", "Air Temperature" }, // Celsius
			{"WindSpeedMS", "Wind speed" }, // m/s 
			{"Humidity", "Relative humidity"}, // %
			{"PrecipitationAmount", "Precipitation Amount" }, // mm
			{ "TotalCloudCover", "Cloudiness"},// % 
			{"TA_PT1H_AVG", "Average temperature"},
			{ "TA_PT1H_MAX", "Max temperature"},
			{ "TA_PT1H_MIN","Min temperature" }
		};
		// Pairing parameters and TypeFormat structs
		public static readonly Dictionary<string, TypeFormat> FORMAT_PARAMS = new Dictionary<string, TypeFormat>()
		{
			{"t2m", TempStruct},
			{"Temperature", TempStruct},
			{"TA_PT1H_AVG", TempStruct},
			{"TA_PT1H_MAX", TempStruct},
			{"TA_PT1H_MIN", TempStruct},
			{"ws_10min", WindStruct},
			{"WindSpeedMS", WindStruct},
			{"rh", HumidityStruct},
			{"Humidity", HumidityStruct},
			{"r_1h", PrecipitationStruct},
			{"PrecipitationAmount", PrecipitationStruct},
			{"n_man", CloudinessStruct},
			{"TotalCloudCover", CloudinessStruct},
			{"Precipitation1h", PrecipitationStruct}
		};


		static readonly string SERVER_URL = "http://opendata.fmi.fi/wfs?service=WFS&version=2.0.0&request=getFeature&";

		// MINIMUM REQUIRMENTS QUERIES
		//	fmi::observations::weather::simple	
		//	fmi::forecast::hirlam::surface::point::simple

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
			options.Add("timestep", Timestep);
			options.Add("storedquery_id", query);
			options.Add("place", Place);
			options.Add("parameters", Parameters);
			options.Add("starttime", StartTime);
			options.Add("endtime", EndTime);


			string request = $"{SERVER_URL}" + string.Join("&", options.ToList().Select(x =>
			  x.Key + "=" + x.Value));
			return request;
		}
		/*
		 * This is the function responsible for sending the request to the API. 
		 * The url that other functions have created is taken as an argument. 
		 * Boolean flag is just for warning display. 
		 * This function is called multiple times if the time period is longer than a week. 
		 * Returns a list of plots. 
		 */
		public static async Task<List<IDataSeries>> GetSingleData(string url, bool is_first_chunk = false)
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
					throw new Exception("None of the selected datatypes is available for this area!");
				}
			}

			var result_nodes = doc.SelectNodes("//om:result", mng);
			List<IDataSeries> plots = new List<IDataSeries>();

			// List of formats to display which graphs are missing
			List<string> missing_graphs = new List<string>();
			List<string> found_graphs = new List<string>();

			foreach (XmlNode result in result_nodes)
			{
				// Creating the series and setting all the necessary data to plot
				List<Tuple<DateTime, IData>> series = new List<Tuple<DateTime, IData>>();
				TypeFormat typeformat = GetTypeFormat(result, mng);
				DataFormat format = GetFormat(typeformat);

				// Sorting plots by associated parameter description
				string plot_parameter = GetParameter(result, mng);
				string plot_name = PARAMETERS[plot_parameter];

				// With the above setting we can create an instance of DataSeries
				var plot = DataSeriesFactory.CreateDataSeries(plot_name, format, series); ;

				// VALUETIMEPAIR				
				var PairLst = result.SelectNodes(".//wml2:MeasurementTVP", mng);

				// Flag checks if there are non-NaN values, this is a very lazy solution but it works
				bool all_NaN = true;

				foreach (XmlNode TimeValuePair in PairLst)
				{
					DateTime time = Convert.ToDateTime(TimeValuePair.SelectSingleNode(".//wml2:time", mng).InnerText);
					double.TryParse(TimeValuePair.SelectSingleNode(".//wml2:value", mng).InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);

					if (Double.IsNaN(value))
					{	
						continue;
					}

					// If this is reached then at least 1 value isn't NaN
					all_NaN = false;

					// Getting the correct weather class, i.e. Temperature, Humidity etc based on parameter
					dynamic data = GetType(typeformat, value);
					Tuple<DateTime, IData> plotPoint = new Tuple<DateTime, IData>(time, data);
					series.Add(plotPoint);
				}
				if (all_NaN)
				{
					if (!missing_graphs.Contains(plot_name))
					{
						missing_graphs.Add(plot_name);
					}
				}
				else
				{
					if (!found_graphs.Contains(plot_name))
					{
						found_graphs.Add(plot_name);
					}
					plots.Add(plot);
				}
			}

			// Showing the user which graphs are missing (if any)
			if (missing_graphs.Any() && is_first_chunk)
			{
				TellAboutGraphs(missing_graphs, found_graphs);
			}

			return plots;
		}
		/*
		 * Splits the FMI request, if it's too long and calls GetSingleData n times, where n
		 * is number of splits. Combines all fetched graphs based on parameter
		 * Returns a dictionary of combined graphs sorted by parameter. 
		 */
		public static Dictionary<string, IDataSeries> GetAllData(DateTime startTime, DateTime endTime, int interval,
			string graphName, string cityName, string parameters, WeatherType.ParameterEnum parameterType)
        {
			// This dict is returned
			Dictionary<string, IDataSeries> combined_graphs = new Dictionary<string, IDataSeries>();
			if (TimeHandler.ForecastTooFar(startTime)) { return combined_graphs; }

			string step = interval.ToString();
			Timestep = step;
			if (TimeHandler.DataTooBig(startTime, endTime, interval)) { return combined_graphs; }

			List<Tuple<DateTime, DateTime>> timepairs = TimeHandler.SplitFMIRequest(startTime, endTime);
			foreach (var timepair in timepairs)
			{
				StartTime = TimeHandler.ConvertToLocalTime(timepair.Item1).ToString("yyyy-MM-ddTHH:mm:ssZ");
				EndTime = TimeHandler.ConvertToLocalTime(timepair.Item2).ToString("yyyy-MM-ddTHH:mm:ssZ");

				Place = cityName;
				Parameters = parameters;

				string query;
				if (parameterType == WeatherType.ParameterEnum.Forecast)
				{
					query = BuildQuery("forecast::hirlam::surface::point");
				}
				else
				{
					query = BuildQuery("observations::weather");
				}
				string request = BuildRequest(query);
				Console.WriteLine(request);

				// This flag makes sure that the missing graphs warning is shown only once
				bool is_first = false;
				if(timepairs.First() == timepair)
                {
					is_first = true;
                }

				var series_list_task = Task.Run(() => GetSingleData(request, is_first));
				series_list_task.Wait();
				var series_list = series_list_task.Result;

				foreach (var series in series_list)
				{
					AddToDict(ref combined_graphs, series);
				}
			}
			return combined_graphs;

		}
		/*
		 * Helper function defines custom behavior for GetAllData. 
		 * If the key is already present in the dict, combines its value
		 * to the incoming series
		 */
		private static void AddToDict(ref Dictionary<string, IDataSeries> dict, IDataSeries plot)
		{
			if (dict.ContainsKey(plot.Name))
			{
				var series = dict[plot.Name];
				series.Series.AddRange(plot.Series);
			}
			else
			{
				dict.Add(plot.Name, plot);
			}
		}


		/*	
		 *	Creates an XML manager for parsing 
		 */
		private static XmlNamespaceManager CreateManager(XmlDocument doc)
		{
			var mng = new XmlNamespaceManager(doc.NameTable);

			mng.AddNamespace("om", "http://www.opengis.net/om/2.0");
			mng.AddNamespace("wml2", "http://www.opengis.net/waterml/2.0");
			mng.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			mng.AddNamespace("gml", "http://www.opengis.net/gml/3.2");

			return mng;
		}

		/* Creates and returns type-format pair (TypeFormat) based 
		 * on the provided format
		 */
		private static TypeFormat GetTypeFormat(XmlNode node, XmlNamespaceManager mng)
		{
			string parameter = GetParameter(node, mng);
			TypeFormat typeformat = FORMAT_PARAMS[parameter];
			return typeformat;
		}

		/*
		 * Parses the XML node for parameter and returns it 
		 */
		private static string GetParameter(XmlNode node, XmlNamespaceManager mng)
        {
			var param_id = node.SelectSingleNode(".//wml2:MeasurementTimeseries", mng);
			string attribute = param_id.Attributes["gml:id"].Value;
			string parameter = attribute.Split('-').Last();
			return parameter;
		}
		/*
		 * Returns the format of TypeFormat
		 */
		private static DataFormat GetFormat(TypeFormat typeformat)
		{
			return typeformat.Format;
		}
		/*
		 * Dynamically creates an instance with a value of "value" of the correct type
		 * based on the TypeFormat. 
		 */
		private static dynamic GetType(TypeFormat typeformat, double value)
		{
			var type = typeformat.Datatype;
			dynamic data = Activator.CreateInstance(type, value);
			return data;
		}

		// Displays a warning if there are missing graphs
		private static void TellAboutGraphs(List<string> missing_graphs, List<string> found_graphs)
		{
			string miss_graphs = AddDivs(missing_graphs);
			string disp_graphs = "";

			if (found_graphs.Any())
			{
				disp_graphs += "Showing only " + AddDivs(found_graphs) + " data";
			}

			MessageBox.Show($"Requested {miss_graphs} data is missing for this area. {disp_graphs}");
		}

		/*
		 * Makes missing graphs warning message more readable
	 	 */
		private static string AddDivs(List<string> graphs)
		{
			string listed_graphs = "";
			string div;
			foreach (var graph in graphs)
			{
				if (graph == graphs.First())
				{
					div = "";
				}
				else if (graph == graphs.Last() && graph != graphs.First())
				{
					div = " and ";
				}
				else
				{
					div = ", ";
				}
				listed_graphs += div;
				listed_graphs += graph.ToString();
			}
			return listed_graphs;
		}

	}
}
