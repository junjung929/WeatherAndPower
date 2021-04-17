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
		/**
		 * Unique ID of the graph
		 */
		public int Id { get; set; }

		/**
		 * The name of the graph
		 */
		public string Name { get; set; }

		/**
		 * the display color of the graph
		 */
		public byte[] Color { get; set; }

		/**
		 * Determines if the series is selected in the series list (in sidebar)
		 */
		public bool IsSelected { get; set; }

		/**
		 * Determines if the series is visible in the graph
		 */
		public bool IsVisible { get; set; }

		/** 
		 * Determines if a plot should be considered for comparison
		 * Our implementation only allows comparing of specifically sourced
		 * power data, so that is the only type of dataseries that should have this
		 * as true
		 */
		public bool IsComparable { get; set; }

		/**
		 * What kind of data this DataSeries contains
		 */
		public DataFormat Format { get; set; }

		/**
		 * The actual data as a time series object
		 */
		public List<Tuple<DateTime, IData>> Series { get; set; }

		/**
		 * returns the minimum DateTime in this series that has a value (series start date)
		 */
		public DateTime Minimum { get; } 
		
		/**
		 * returns the maximum DateTime in this series that has a value (series end date)
		 */
		public DateTime Maximum { get; }

		/**
		 * Gets the value of a single point in the graph as a DataPoint object. 
		 * Interpolates between the nearest values if an exact match is not found
		 */
		public DataPoint GetDataPoint(DateTime point);

		/**
		 * does what it says on the label
		 */
		public void RandomizeColor();

		public void SetColor(byte r, byte g, byte b);

	}
}
