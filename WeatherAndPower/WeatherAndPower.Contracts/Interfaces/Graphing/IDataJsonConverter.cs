using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class IDataJsonConverter : JsonConverter<IData>
	{
		public override IData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			reader.Read();
			DataFormat format = (DataFormat)reader.GetInt32();
			reader.Read();
			var data = Globals.GetIDataFromDataFormat(format, reader.GetDouble());
			reader.Read();
			return data;
		}

		public override void Write(Utf8JsonWriter writer, IData data, JsonSerializerOptions options)
		{
			writer.WriteStartArray();

			DataFormat format = Globals.GetDataFormatOfData(data);
			writer.WriteNumberValue((int)format);
			writer.WriteNumberValue(data.Value);

			writer.WriteEndArray();
		}
	}
}
