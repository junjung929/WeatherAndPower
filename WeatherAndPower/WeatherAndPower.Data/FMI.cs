using System;
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

		public static IDataSeriesFactory DataSeriesFactory { get; set; }

		public static string Place { get; set; }
		public static string Query { get; set; }
		public static string Parameters { get; set; }
		public static string StartTime { get; set; }
		public static string EndTime { get; set; }
		public static string Timestep { get; set; } = "60"; // Default value




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
		/*
		public struct TypeFormat
		{
			public DataFormat Format;
			public Type Datatype;

			public TypeFormat(DataFormat form, Type type)
			{
				Format = form;
				Datatype = type;
			}
		};

		public static TypeFormat TempStruct = new TypeFormat(DataFormat.Temperature, typeof(Temperature));
		public static TypeFormat WindStruct = new TypeFormat(DataFormat.Wind, typeof(WindSpeed));
		public static TypeFormat HumidityStruct = new TypeFormat(DataFormat.Humidity, typeof(Humidity));
		public static TypeFormat CloudinessStruct = new TypeFormat(DataFormat.Cloudiness, typeof(Cloudiness));
		public static TypeFormat PrecipitationStruct = new TypeFormat(DataFormat.Precipitation, typeof(Precipitation));

		*/

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

		public static async Task<List<IDataSeries>> GetData(string url)
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
					throw new Exception("None of selected datatypes is available for this area!");
				}
			}

			var result_nodes = doc.SelectNodes("//om:result", mng);
			List<IDataSeries> plots = new List<IDataSeries>();

			// These flags make sure that warning message about missing graphs/values is shown only once
			bool already_shown_graphs = false;
			bool already_shown_values = false;

			// List of formats to display which graphs are missing
			List<DataFormat> missing_graphs = new List<DataFormat>();
			List<DataFormat> found_graphs = new List<DataFormat>();

			foreach (XmlNode result in result_nodes)
			{

				List<Tuple<DateTime, IData>> series = new List<Tuple<DateTime, IData>>();
				TypeFormat typeformat = GetTypeFormat(result, mng);
				DataFormat format = GetFormat(typeformat);


				var plot = DataSeriesFactory.CreateDataSeries(Place, format, series);

				// VALUETIMEPAIR				
				var PairLst = result.SelectNodes(".//wml2:MeasurementTVP", mng);

				foreach (XmlNode TimeValuePair in PairLst)
				{
					DateTime time = Convert.ToDateTime(TimeValuePair.SelectSingleNode(".//wml2:time", mng).InnerText);
                    double.TryParse(TimeValuePair.SelectSingleNode(".//wml2:value", mng).InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
					if (Double.IsNaN(value))
					{
						// If the first one is NaN then the whole set is faulty
						if (!series.Any())
						{
							already_shown_graphs = true;
							missing_graphs.Add(format);
							break;
						}
						else
						{
							if (!already_shown_values && !already_shown_graphs)
							{
								MessageBox.Show("Some of the reuqested values are missing!");
								already_shown_values = true;
							}
							continue;
						}

					}

					// Getting the correct weather class, i.e. Temperature, Humidity etc based on parameter
					dynamic data = GetType(typeformat, value);

					Tuple<DateTime, IData> plotPoint = new Tuple<DateTime, IData>(time, data);
					series.Add(plotPoint);

					//Console.WriteLine(time);
					//Console.WriteLine(value);
				}
				if (!missing_graphs.Contains(format))
				{
					found_graphs.Add(format);
					plots.Add(plot);
				}
			}

			// Showing the user which graphs are missing (if any)
			if (missing_graphs.Any())
			{
				TellAboutGraphs(missing_graphs, found_graphs);
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
		/// Returns the format - type struct based on parameter
		/// </summary>
		/// <param name="node"></param>
		/// <param name="mng"></param>
		/// <returns></returns>
		private static TypeFormat GetTypeFormat(XmlNode node, XmlNamespaceManager mng)
		{
			var param_id = node.SelectSingleNode(".//wml2:MeasurementTimeseries", mng);
			string attribute = param_id.Attributes["gml:id"].Value;
			string parameter = attribute.Split('-').Last();
			TypeFormat typeformat = FORMAT_PARAMS[parameter];
			Console.WriteLine(typeformat.Format);
			return typeformat;
		}

		private static DataFormat GetFormat(TypeFormat typeformat)
		{
			return typeformat.Format;
		}

		private static dynamic GetType(TypeFormat typeformat, double value)
		{
			var type = typeformat.Datatype;
			dynamic data = Activator.CreateInstance(type, value);
			return data;
		}

		private static void TellAboutGraphs(List<DataFormat> missing_graphs, List<DataFormat> found_graphs)
		{
			string miss_graphs = AddDivs(missing_graphs);
			string disp_graphs = "";

			if (found_graphs.Any())
			{
				disp_graphs += "Showing only " + AddDivs(found_graphs) + " data";
			}

			MessageBox.Show($"Requested {miss_graphs} data is missing for this area. {disp_graphs}");
		}

		private static string AddDivs(List<DataFormat> graphs)
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


