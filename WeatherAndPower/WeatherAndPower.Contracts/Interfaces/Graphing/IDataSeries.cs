using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IDataSeries : INotifyPropertyChanged
	{
		[JsonIgnore]
		public int Id { get; set; }

		public string Name { get; set; }

		public byte[] Color { get; set; }

		public bool IsSelected { get; set; }

		public bool IsVisible { get; set; }

		public bool IsComparable { get; set; }

		public DataFormat Format { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }

		public DateTime Minimum { get; } 
		
		public DateTime Maximum { get; }

		public DataPoint GetDataPoint(DateTime point);

		public void RandomizeColor();

		public void SetColor(byte r, byte g, byte b);

	}
}
