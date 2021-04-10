using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows;
using System.Collections.ObjectModel;
using WeatherAndPower.Contracts;
using System.Collections.Specialized;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Markup;
using System.IO;
using DataFormat = WeatherAndPower.Contracts.DataFormat;
using System.Windows.Input;
using System.Windows.Controls.DataVisualization;
using System.Windows.Media.Imaging;

namespace WeatherAndPower.UI
{
	public class CustomChart : Chart, ICustomChart
	{
		private DataFormat _Formats = 0;

		public new IEnumerable<DataSeries> Series
		{
			get { return (IEnumerable<DataSeries>)GetValue(SeriesProperty); }
			set {
				if (value is ObservableCollection<DataSeries>) {
					((ObservableCollection<DataSeries>)value).CollectionChanged += SeriesChanged;
				}
				SetValue(SeriesProperty, value);
			}
		}
		public static readonly DependencyProperty SeriesProperty =
			DependencyProperty.Register(
				"Series",
				typeof(IEnumerable<DataSeries>),
				typeof(CustomChart),
				new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					new PropertyChangedCallback(OnSeriesChanged))
				);

		private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is ObservableCollection<DataSeries>) {
				((ObservableCollection<DataSeries>)e.NewValue).CollectionChanged += ((CustomChart)d).SeriesChanged;
			}
		}

		#region Axis stuff
		private LinearAxis GetAxis(DataFormat format)
		{
			return (LinearAxis)this.Axes.First(a => ((FrameworkElement)a).Name == Globals.AxisNames[format]);
		}

		private void RefreshAxisVisibilities()
		{
			TemperatureAxis.Visibility = ShowTemperatureAxis ? Visibility.Visible : Visibility.Collapsed;
			PowerAxis.Visibility = ShowPowerAxis ? Visibility.Visible : Visibility.Collapsed;
			WindAxis.Visibility = ShowWindAxis ? Visibility.Visible : Visibility.Collapsed;
			CloudinessAxis.Visibility = ShowCloudinessAxis ? Visibility.Visible : Visibility.Collapsed;
		}

		#region X Axis

		private TimeAxis X = new TimeAxis()
		{
			Name = "TimeAxis",
		};


		public bool ShowLegend
		{
			set {
				if (!value) {
					Style style = new Style(typeof(Legend));
					style.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
					style.Setters.Add(new Setter(WidthProperty, 0.0));
					style.Setters.Add(new Setter(HeightProperty, 0.0));
					LegendStyle = style;
				} else {
					LegendStyle = null;
				}
			}
		}


		private TimeSpan? _XInterval = null;

		public TimeSpan? XInterval
		{
			get {
				return _XInterval;
			}
			set {
				if (_XInterval != value) {
					_XInterval = value;
					//NotifyPropertyChanged("XInterval");
				}
			}
		}


		#endregion

		#region Temperature

		//public IAxis TemperatureAxis
		//{
		//	get { return _Chart.Axes.First(e => ((LinearAxis)e).Name == "TemperatureAxis"); }
		//}
		public LinearAxis TemperatureAxis = new LinearAxis()
		{
			Name = Globals.AxisNames[DataFormat.Temperature],
			Orientation = AxisOrientation.Y,
			Visibility = Visibility.Collapsed,
			Title = "Temperature"
		};

		public bool ShowTemperatureAxis
		{
			get { return (_Formats & DataFormat.Temperature) != 0; }
		}


		#endregion
		#region Power

		public LinearAxis PowerAxis = new LinearAxis()
		{
			Name = Globals.AxisNames[DataFormat.Power],
			Orientation = AxisOrientation.Y,
			Visibility = Visibility.Collapsed,
			Title = "Power"
		};

		private bool ShowPowerAxis
		{
			get { return (_Formats & DataFormat.Power) != 0; }
		}

		#endregion

		#region Cloudiness

		public LinearAxis CloudinessAxis = new LinearAxis()
		{
			Name = Globals.AxisNames[DataFormat.Cloudiness],
			Orientation = AxisOrientation.Y,
			Visibility = Visibility.Collapsed,
			Title = "Cloudiness"
		};

		private bool ShowCloudinessAxis
		{
			get { return (_Formats & DataFormat.Cloudiness) != 0; }
		}

		#endregion

		#region Wind

		public LinearAxis WindAxis = new LinearAxis()
		{
			Name = Globals.AxisNames[DataFormat.Wind],
			Orientation = AxisOrientation.Y,
			Visibility = Visibility.Collapsed,
			Title = "Wind"
		};

		public bool ShowWindAxis
		{
			get { return (_Formats & DataFormat.Wind) != 0; }
		}

		#endregion

		#endregion

		#region Saving

		public bool Save(string fileName)
		{
			RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
				(int)ActualWidth,
				(int)ActualHeight,
				96.0,
				96.0,
				PixelFormats.Pbgra32);
			var vis = new DrawingVisual();

			var content = vis.RenderOpen();
			content.DrawRectangle(Brushes.White, null, new Rect(0,0, (int)ActualWidth, (int)ActualHeight));
			content.Close();

			renderBitmap.Render(vis);
			renderBitmap.Render(this);

			using (FileStream stream = new FileStream(fileName, FileMode.Create)) {
				JpegBitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
				encoder.Save(stream);
			}
			return true;
		}

		#endregion

		public CustomChart()
		{

			Axes.Add(X);

			Axes.Add(TemperatureAxis);
			Axes.Add(PowerAxis);
			Axes.Add(CloudinessAxis);
			Axes.Add(WindAxis);

			ShowLegend = false;
		}

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


		private void Plot(DataSeries data) {
			LineSeries series = new LineSeries();
			series.ItemsSource = data.Series;
			series.DependentValuePath = "Item2.Value";
			series.IndependentValuePath = "Item1";
			series.Title = data.Name;
			
			series.DataPointStyle = GetLineStyle(data.Color);
			series.DependentRangeAxis = GetAxis(data.Format);
			series.IndependentAxis = X;

			series.MouseDown += SeriesClicked;
			series.MouseEnter += SeriesHover;
			series.MouseLeave += SeriesHover;

			series.Background = new SolidColorBrush(Colors.Red);

			base.Series.Add(series);
			data.Id = series.GetHashCode();
		}
		public event MouseEventHandler SeriesMouseEnter;
		public event MouseButtonEventHandler SeriesClicked;
		public event MouseEventHandler SeriesHover;

		private void Clear()
		{
			base.Series.Clear();
		}

		private void Remove(int id)
		{
			try {
				var item = base.Series.First(i => i.GetHashCode() == id);
				base.Series.Remove(item);
			} catch (InvalidOperationException) { }
		}


		private void SeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove ||
				e.Action == NotifyCollectionChangedAction.Replace) {
				_Formats = 0;
				for (int i = 0; i < e.OldItems.Count; i++) {
					Remove(((DataSeries)e.OldItems[i]).Id);
				}
				var coll = (IEnumerable<DataSeries>)sender;
				for (int i = 0; i < coll.Count(); i++) {
					_Formats |= coll.ElementAt(i).Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Add ||
				e.Action == NotifyCollectionChangedAction.Replace) {
				for (int i = 0; i < e.NewItems.Count; i++) {
					Plot((DataSeries)e.NewItems[i]);
					_Formats |= ((DataSeries)e.NewItems[i]).Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				Clear();
				_Formats = 0;
				if (((ObservableCollection<DataSeries>)sender).Count > 0) {
					foreach (var item in ((ObservableCollection<DataSeries>)sender)) {
						Plot(item);
						_Formats |= item.Format;
					}
				}
			}
			RefreshAxisVisibilities();
		}
	}
}
