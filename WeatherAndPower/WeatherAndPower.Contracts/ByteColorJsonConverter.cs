using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class ByteColorJsonConverter : JsonConverter<byte[]>
	{
		public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var colorString = reader.GetString();
			var color = new byte[3];
			for (int i = 0; i < 6; i += 2) {
				color[i/2] = Convert.ToByte(colorString.Substring(i+1, 2), 16);
			}

			return color;

		}

		public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
		{
			var colorString = BitConverter.ToString(value).Replace("-", string.Empty);
			writer.WriteStringValue(colorString.Insert(0, "#"));
		}
	}
}
