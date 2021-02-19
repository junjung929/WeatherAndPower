using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WeatherAndPower.Data
{
	public class BaseHttpClient
	{
		protected static readonly HttpClient _client = new HttpClient();

		public static async Task Test()
		{
			await Task.Delay(3000);
			try {
				HttpResponseMessage res = await _client.GetAsync("http://opendata.fmi.fi/wfs?service=WFS&request=getCapabilities&");
				string body = await res.Content.ReadAsStringAsync();


				Console.WriteLine("here it comes");
				Console.WriteLine(body);


			}
			catch (HttpRequestException e) {
				Console.WriteLine("\nException Caught");
				Console.WriteLine("Message :{0} ", e.Message);
			}
		}
		//public static async string GET(string url)
		//{
		//	await Task.Delay(3000);
		//	try {
		//		HttpResponseMessage res = await _client.GetAsync("http://opendata.fmi.fi/wfs?service=WFS&request=getCapabilities&");
		//		string body = await res.Content.ReadAsStringAsync();
		//		// HttpResponseMessage res = await _client.GetAsync(url);
		//		return body;
		//	}
		//	catch (HttpRequestException e) {
		//		Console.WriteLine("\nException Caught");
		//		Console.WriteLine("Message :{0} ", e.Message);
		//		throw e;
		//	}
		//}

		public static async void GET(string url)
		{
			var body = await _client.GetStringAsync(url);

		}
	}
}
