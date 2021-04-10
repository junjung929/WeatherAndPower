using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace WeatherAndPower.Contracts
{
	public class DataSeriesJsonConverter : JsonConverter<DataSeries>
	{
		public override DataSeries Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			while (reader.Read()) {
				if (reader.TokenType == JsonTokenType.EndObject) throw new JsonException();
				switch(reader.TokenType) {
					
				}
			}
			return new DataSeries("test", DataFormat.Wind, new List<Tuple<DateTime, IData>>());
		}

		public override void Write(Utf8JsonWriter writer, DataSeries value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			foreach (PropertyInfo prop in value.GetType().GetProperties()) {
				var propertyName = prop.Name;
				var propertyType = prop.PropertyType;
				var propertyValue = prop.GetValue(value);

				writer.WritePropertyName(propertyName);

				JsonSerializer.Serialize(writer, propertyValue, options);

				
			}
		}
	}
}
