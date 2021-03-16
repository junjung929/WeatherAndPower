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
using System.Windows.Media;

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

		private Contracts.DataFormat _DataHosted = 0;

		public ObservableCollection<DataSeries> Data 
		{
			get { return Model.Data; }
		}
		private Chart _Chart { get; set; }

		private Style GetLineStyle(byte[] color)
		{
			Color col1 = Color.FromRgb(
				(byte)Math.Min(color[0]+10,255),
				(byte)Math.Min(color[1]+10,255),
				(byte)Math.Min(color[2]+10,255));
			Color col2 = Color.FromRgb(
				(byte)Math.Max(color[0]-10,0),
				(byte)Math.Max(color[1]-10,0),
				(byte)Math.Max(color[2]-10,0));
			var gradient = new LinearGradientBrush() { EndPoint = new Point(0, 0), StartPoint = new Point(0, 1) };
			gradient.GradientStops.Add(new GradientStop(col1, 0));
			gradient.GradientStops.Add(new GradientStop(col2, 1));
			Setter colorSetter = new Setter()
			{
				Property = LineDataPoint.BackgroundProperty,
				Value = gradient
			};
			Setter opacitySetter = new Setter()
			{
				Property = LineDataPoint.OpacityProperty,
				Value = 0.0
			};
			Style style = new Style();
			style.Setters.Add(colorSetter);
			style.Setters.Add(opacitySetter);

			return style;
		}
		private void Plot(DataSeries data)
		{
			LineSeries series = new LineSeries();
			series.ItemsSource = data.Series;
			series.DependentValuePath = "Item2.Value";
			series.IndependentValuePath = "Item1.Ticks";
			series.Title = data.Name;


			series.DataPointStyle = GetLineStyle(data.Color);
			//series.DataPointStyle.Setters.Add(colorSetter);

			if (data.Format == Contracts.DataFormat.Temperature) {
				series.DependentRangeAxis = (IRangeAxis)TemperatureAxis;
			} else if (data.Format == Contracts.DataFormat.Power) {
				series.DependentRangeAxis = (IRangeAxis)PowerAxis;
			} else if (data.Format == Contracts.DataFormat.Cloudiness) {
				series.DependentRangeAxis = (IRangeAxis)CloudinessAxis;
			} else if (data.Format == Contracts.DataFormat.Wind) {
				series.DependentRangeAxis = (IRangeAxis)WindAxis;
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

		#region X Axis

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

		#endregion

		#region Temperature

		public IAxis TemperatureAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "TemperatureAxis"); }
		}

		public bool ShowTemperature
		{
			get { return (_DataHosted & Contracts.DataFormat.Temperature) != 0; }
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

		#endregion

		#region Power

		public IAxis PowerAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "PowerAxis"); }
		}

		public bool ShowPower
		{
			get { return (_DataHosted & Contracts.DataFormat.Power) != 0; }
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

		#region Cloudiness

		public IAxis CloudinessAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "CloudinessAxis"); }
		}

		public bool ShowCloudiness
		{
			get { return (_DataHosted & Contracts.DataFormat.Cloudiness) != 0; }
		}

		private int? _CloudinessInterval = null;

		public int ? CloudinessInterval
		{
			get { return _CloudinessInterval; }
			set {
				if (_CloudinessInterval != value) {
					_CloudinessInterval = value;
					NotifyPropertyChanged("CloudinessInterval");
				}
			}
		}

		private int? _CloudinessMin = null;

		public int? CloudinessMin
		{
			get { return _CloudinessMin; }
			set {
				if (_CloudinessMin != value) {
					_CloudinessMin = value;
					NotifyPropertyChanged("CloudinessMin");
				}
			}
		}

		private int? _CloudinessMax = null;

		public int? CloudinessMax
		{
			get { return _CloudinessMax; }
			set {
				if (_CloudinessMax != value) {
					_CloudinessMax = value;
					NotifyPropertyChanged("CloudinessMax");
				}
			}
		}

		#endregion

		#region Wind

		public IAxis WindAxis
		{
			get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "WindAxis"); }
		}

		public bool ShowWind
		{
			get { return (_DataHosted & Contracts.DataFormat.Wind) != 0; }
		}
	
		private int? _WindInterval = null;

		public int ? WindInterval
		{
			get { return _WindInterval; }
			set {
				if (_WindInterval != value) {
					_WindInterval = value;
					NotifyPropertyChanged("WindInterval");
				}
			}
		}

		private int? _WindMin = null;

		public int? WindMin
		{
			get { return _WindMin; }
			set {
				if (_WindMin != value) {
					_WindMin = value;
					NotifyPropertyChanged("WindMin");
				}
			}
		}

		private int? _WindMax = null;

		public int? WindMax
		{
			get { return _WindMax; }
			set {
				if (_WindMax != value) {
					_WindMax = value;
					NotifyPropertyChanged("WindMax");
				}
			}
		}

		#endregion

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
				_DataHosted = 0;
				foreach (var item in ((ObservableCollection<DataSeries>)sender)) {
					_DataHosted |= item.Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace) {
				for (int i = 0; i < e.NewItems.Count; i++) {
					Plot((DataSeries)e.NewItems[i]);
					_DataHosted |= ((DataSeries)e.NewItems[i]).Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				Clear();
				_DataHosted = 0;
				if (((ObservableCollection<DataSeries>)sender).Count > 0) {
					foreach (var item in ((ObservableCollection<DataSeries>)sender)) {
						Plot(item);
						_DataHosted |= item.Format;
					}
				}
			}
			NotifyPropertyChanged("ShowTemperature");
			NotifyPropertyChanged("ShowPower");
			NotifyPropertyChanged("ShowWind");
			NotifyPropertyChanged("ShowCloudiness");
		}
	}
}
