using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WeatherAndPower.Data
{
	public class FMI : BaseHttpClient
	{
		static string SERVER_URL = "http://opendata.fmi.fi/wfs";

		private static string GetRequest(string query, string place)
		{
			var options = new Dictionary<string, string>();
			options.Add("service", "WFS");
			options.Add("version", "2.0.0");
			options.Add("request", "getFeature");
			options.Add("storedquery_id", "fmi::forecast::harmonie::surface::point::simple");
			options.Add("place", place);
			string request = $"{SERVER_URL}?" + string.Join( "&", options.ToList().Select( x =>
				x.Key + "=" + x.Value
			));
			return request;
		}
		public static async void GetTemperature()
		{
			var request = GetRequest("", "hervanta");
			Console.WriteLine($"Sending request: {request}");

			var response = await _client.GetAsync(request);
			var body = await response.Content.ReadAsStringAsync();

			var doc = new XmlDocument();
			doc.LoadXml(body);
			var mng = new XmlNamespaceManager(doc.NameTable);
			mng.AddNamespace("wfs", "http://www.opengis.net/wfs/2.0");
			mng.AddNamespace("BsWfs", "http://xml.fmi.fi/schema/wfs/2.0");
			mng.AddNamespace("gml", "http://www.opengis.net/gml/3.2");

			var nodes = doc.SelectNodes("//wfs:member", mng);

			foreach (XmlNode node in nodes) {
				var name = node.SelectSingleNode(".//BsWfs:ParameterName", mng).InnerText;
				var value = node.SelectSingleNode(".//BsWfs:ParameterValue", mng).InnerText;
				var pos = node.SelectSingleNode(".//gml:pos", mng).InnerText;
				var time = node.SelectSingleNode(".//BsWfs:Time", mng).InnerText;

				if (name == "Temperature")
					Console.WriteLine($"{name} = {value} at {pos} at {time}");
			}
		}
	}
}
