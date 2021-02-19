using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace WeatherAndPower.UI
{

	public class DataPlotViewModel : ViewModelBase
	{

		private Chart _chart { get; set; }

		//public ObservableCollection<DataPointSeries> Data { get; set; } = new ObservableCollection<DataPointSeries>();
		public ObservableCollection<List<Point>> Data { get; set; } = new ObservableCollection<List<Point>>();
		public List<Point> Line { get; set; } = new List<Point>();

		///<summary>
		///Allowed types inherit from DataPointSeries
		///<list type="bullet">
		///<item><term>LineSeries</term><description> Plots data using a line</description></item>
		///<item><term>ColumnSeries</term><description> Plots data in vertical columns</description></item>
		///<item><term>BarSeries</term><description> Plots data in horizontal bars</description></item>
		///<item><term>AreaSeries</term><description> Plots a line with an area</description></item>
		///<item><term>BubbleSeries</term><description> Plots bubbles</description></item>
		///<item><term>PieSeries</term><description> Plots a pie graph</description></item>
		///<item><term>ScatterSeries</term><description> Plots separate points</description></item>
		///</list>
		///</summary>
		public void Plot<T>(List<Point> data, string legendName = null) where T : DataPointSeries, new()
		{
			T series = new T();
			series.ItemsSource = data;
			series.DependentValuePath = "Y";
			series.IndependentValuePath = "X";
			series.Title = legendName;
			_chart.Series.Add(series);
			//Data.Add(series);
		}

		private int? _XMin = 0;
		public int? XMin
		{
			get { return _XMin; }
			set {
				if (_XMin != value) {
					_XMin = value;
					NotifyPropertyChanged("XMin");
				}
			}
		}

		private int? _XMax = null;
		public int? XMax
		{
			get { return _XMax; }
			set {
				if (_XMax != value) {
					_XMax = value;
					NotifyPropertyChanged("XMax");
				}
			}
		}

		public DataPlotViewModel(FrameworkElement view)
		{
			var chart = view.FindName("theChart");
			if (chart != null) {
				_chart = chart as Chart;
			}


			Plot<LineSeries>(Line, "Test Line");
			//Plot<PieSeries>(Line);

			Line.Add(new Point(0, 10));
			Line.Add(new Point(1, 15));
			Line.Add(new Point(2, 5));
			Line.Add(new Point(3, 12));
			Line.Add(new Point(4, 20));

			NotifyPropertyChanged("Line");
		}
	}
}
