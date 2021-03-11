using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{

	public class DataPlotViewModel : ViewModelBase
	{

		private IDataPlotModel _Model;
		public IDataPlotModel Model
		{
			get { return _Model; }
			private set {
				if (_Model != value) {
					_Model = value;
				}
			}
		}

		public ObservableCollection<DataSeries> Data 
		{
			get { return Model.Data; }
		}
		private Chart _Chart { get; set; }

		//public ObservableCollection<DataPointSeries> Data { get; set; } = new ObservableCollection<DataPointSeries>();
		//public ObservableCollection<List<Point>> Data { get; set; } = new ObservableCollection<List<Point>>();
		//public List<Point> Line { get; set; } = new List<Point>();

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
		public void Plot<T>(DataSeries data) where T : DataPointSeries, new()
		{
			T series = new T();
			series.ItemsSource = data.Series;
			series.DependentValuePath = "Item2.Value";
			series.IndependentValuePath = "Item1.Ticks";
			series.Title = data.Name;
			data.Id = series.GetHashCode();


			_Chart.Series.Add(series);
		}

		public void Plot(DataSeries data)
		{
			LineSeries series = new LineSeries();
			series.ItemsSource = data.Series;
			series.DependentValuePath = "Item2.Value";
			series.IndependentValuePath = "Item1.Ticks";
			series.Title = data.Name;
			
			if (data.Format == Contracts.DataFormat.Temperature) {
				series.DataPointStyle = _Chart.Resources["TemperatureLine"] as Style;
				series.DependentRangeAxis = (IRangeAxis)TemperatureAxis;
			} else if (data.Format == Contracts.DataFormat.Power) {
				series.DataPointStyle = _Chart.Resources["PowerLine"] as Style;
				series.DependentRangeAxis = (IRangeAxis)PowerAxis;
			}
			data.Id = series.GetHashCode();
			
			_Chart.Series.Add(series);
		}

		private void Clear()
		{
			_Chart.Series.Clear();
		}

		private void Remove(int id)
		{
			var item = _Chart.Series.First(i => i.GetHashCode() == id);
			_Chart.Series.Remove(item);
		}

		#region Axis Properties

		private TimeSpan? _XInterval = null;
		public TimeSpan? XInterval
		{
			get {
				return _XInterval;
			}
			set {
				if (_XInterval != value) {
					_XInterval = value;
					NotifyPropertyChanged("XInterval");
				}
			}
		}
		
		private int? _XMin = null;
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

		public IAxis TemperatureAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "TemperatureAxis"); }
		}

		private int? _TemperatureInterval = null;
		public int ? TemperatureInterval
		{
			get { return _TemperatureInterval; }
			set { if (_TemperatureInterval != value) {
					_TemperatureInterval = value;
					NotifyPropertyChanged("TemperatureInterval");
				}
			}
		}
		private int? _TemperatureMin = null;
		public int? TemperatureMin
		{
			get { return _TemperatureMin; }
			set {
				if (_TemperatureMin != value) {
					_TemperatureMin = value;
					NotifyPropertyChanged("TempetatureMin");
				}
			}
		}

		private int? _TemperatureMax = null;
		public int? TemperatureMax
		{
			get { return _TemperatureMax; }
			set {
				if (_TemperatureMax != value) {
					_TemperatureMax = value;
					NotifyPropertyChanged("TemperatureMax");
				}
			}
		}

		public IAxis PowerAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "PowerAxis"); }
		}
		private int? _PowerInterval = null;
		public int ? PowerInterval
		{
			get { return _PowerInterval; }
			set {
				if (_PowerInterval != value) {
					_PowerInterval = value;
					NotifyPropertyChanged("PowerInterval");
				}
			}
		}
		private int? _PowerMin = null;
		public int? PowerMin
		{
			get { return _PowerMin; }
			set {
				if (_PowerMin != value) {
					_PowerMin = value;
					NotifyPropertyChanged("PowerMin");
				}
			}
		}

		private int? _PowerMax = null;
		public int? PowerMax
		{
			get { return _PowerMax; }
			set {
				if (_PowerMax != value) {
					_PowerMax = value;
					NotifyPropertyChanged("PowerMax");
				}
			}
		}
		#endregion

		public DataPlotViewModel(IDataPlotModel model, FrameworkElement view)
		{
			Model = model;
			_Chart = (Chart)view.FindName("theChart");
			Data.CollectionChanged += DataChanged;
			XInterval = new TimeSpan(0, 1, 0, 0);
			PowerMin = 0;
			PowerMax = 1000;
			TemperatureMin = -40;
			TemperatureMax = 40;
			//XMax = 48;
			//YMax = 50;
		}

		private void DataChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace) {
				for (int i = 0; i < e.OldItems.Count; i++) {
					Remove(((DataSeries)e.OldItems[i]).Id);
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace) {
				for (int i = 0; i < e.NewItems.Count; i++) {
					Plot((DataSeries)e.NewItems[i]);
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				Clear();
				if (((ObservableCollection<DataSeries>)sender).Count > 0) {
					foreach (var item in ((ObservableCollection<DataSeries>)sender)) {
						Plot(item);
					}
				}
			}
		}
	}
}
