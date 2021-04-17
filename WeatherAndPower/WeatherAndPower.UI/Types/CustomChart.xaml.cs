using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DataFormat = WeatherAndPower.Contracts.DataFormat;
using System.Windows.Shapes;
using WeatherAndPower.Contracts;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Controls.DataVisualization;
using System.ComponentModel;
using System.Windows.Controls.DataVisualization.Charting.Primitives;

namespace WeatherAndPower.UI
{
	/// <summary>
	/// Interaction logic for CustomChart.xaml
	/// This class doubles as a sort of viewmodel for our chart.
	/// </summary>
	[TemplatePart(Name = "VerticalCursor", Type = typeof(Grid))]
	[TemplatePart(Name = "CursorCanvas", Type = typeof(Grid))]
	public partial class CustomChart : Chart, ICustomChart, INotifyPropertyChanged
	{
		/**
		 * Bit flag property for tracking what types of data are in the graph
		 */
		private DataFormat _Formats = 0;

		/**
		 * BindableSeries property so we can bind a collection to this in WPF
		 */
		public IEnumerable<IDataSeries> BindableSeries
		{
			get { return (IEnumerable<IDataSeries>)GetValue(BindableSeriesProperty); }
			set {
				if (value is ObservableCollection<IDataSeries>) {
					((ObservableCollection<IDataSeries>)value).CollectionChanged += SeriesChanged;
				}
				SetValue(BindableSeriesProperty, value);
			}
		}
		public static readonly DependencyProperty BindableSeriesProperty =
			DependencyProperty.Register(
				"BindableSeries",
				typeof(IEnumerable<IDataSeries>),
				typeof(CustomChart),
				new FrameworkPropertyMetadata(null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					new PropertyChangedCallback(OnSeriesChanged))
				);

		private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is ObservableCollection<IDataSeries>) {
				((ObservableCollection<IDataSeries>)e.NewValue).CollectionChanged += ((CustomChart)d).SeriesChanged;
			}
		}

		public event MouseButtonEventHandler SeriesClicked;

		/**
		 * This class already derives from Chart, so we cannot derive from AbstractViewModel
		 * Therefore we implement INotifyPropertyChanged here separately
		 */
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}


		#region Axis stuff
		private LinearAxis GetAxis(DataFormat format)
		{
			return (LinearAxis)this.Axes.First(a => {
				var tag = (a as FrameworkElement).Tag as DataFormat?;
				return tag == format;
				});
		}

		/**
		 * Helper method for refreshing all axes visibilities. _Format value is changed multiple times
		 * when new lines are added to the plot, so it does not make sense to refresh axis visibilities
		 * in the _Format setter. This function can be run once all pending data modifications are finished
		 */
		private void RefreshAxisVisibilities()
		{
			NotifyPropertyChanged("ShowTemperatureAxis");
			NotifyPropertyChanged("ShowPowerAxis");
			NotifyPropertyChanged("ShowCloudinessAxis");
			NotifyPropertyChanged("ShowWindAxis");
			NotifyPropertyChanged("ShowHumidityAxis");
			NotifyPropertyChanged("ShowPrecipitationAxis");
		}

		/**
		 * These methods define whether an axis should be visible or not.
		 * By default, axes with no data should be invisible to reduce clutter
		 */
		public bool ShowTemperatureAxis
		{
			get { return (_Formats & DataFormat.Temperature) != 0; }
		}

		public bool ShowPowerAxis
		{
			get { return (_Formats & DataFormat.Power) != 0; }
		}

		public bool ShowCloudinessAxis
		{
			get { return (_Formats & DataFormat.Cloudiness) != 0; }
		}

		public bool ShowWindAxis
		{
			get { return (_Formats & DataFormat.Wind) != 0; }
		}
		public bool ShowHumidityAxis
		{
			get { return (_Formats & DataFormat.Humidity) != 0; }
		}

		public bool ShowPrecipitationAxis
		{
			get { return (_Formats & DataFormat.Precipitation) != 0; }
		}

		#endregion

		#region Saving

		/**
		 * Saves the graph as an image in a jpeg file at the provided path
		 */
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

			try {
				using (FileStream stream = new FileStream(fileName, FileMode.Create)) {
					JpegBitmapEncoder encoder = new JpegBitmapEncoder();
					encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
					encoder.Save(stream);
				}
				return true;
			} catch(IOException) {
				return false;
			}
		}

		#endregion

		/**
		 * This method enables "pick mode" for the chart. Pick mode comes with a custom vertical
		 * cursor and allows the user to pick a specific time point in the graph.
		 */
		public async Task<DateTime> Pick()
		{
			cursorPicked = new TaskCompletionSource<DateTime>();
			ShowCursor = true;
			NotifyPropertyChanged("ShowCursor");
			await cursorPicked.Task;
			ShowCursor = false;
			NotifyPropertyChanged("ShowCursor");

			return cursorPicked.Task.Result;
		}

		public CustomChart()
		{
			InitializeComponent();
		}


		/** 
		 * This method adds an IDataSeries object to the graph as a plot.
		 * It is called automatically every time the BindableSeries property is changed
		 */
		private void Plot(IDataSeries data) {
			var series = new CustomLineSeries();
			series.DataContext = data;
			var nth = (data.Series.Count / Globals.MaximumDataPoints) + 1;
			series.ItemsSource = data.Series.Where((e, i) => i % nth == 0);
			series.DependentValuePath = "Item2.Value";
			series.IndependentValuePath = "Item1";
			series.Title = data.Name;
			
			series.DependentRangeAxis = GetAxis(data.Format);
			series.IndependentAxis = X;

			series.MouseDown += SeriesClicked;

			series.LineColor = Color.FromRgb(data.Color[0], data.Color[1], data.Color[2]);

			series.IsVisibleChanged += OnSeriesVisibilityChanged;

			Series.Add(series);
		}

		/**
		 * This method listens to changes IsVisible property of each series and updates
		 * the _Formats property and axis visibilities based on what series are visible.
		 * Basically this method is responsible for hiding axes that have no visible data
		 */
		private void OnSeriesVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var series = (IDataSeries)((CustomLineSeries)sender).DataContext;
			if ((bool)e.NewValue) {
				_Formats |= series.Format;
			} else {
				_Formats &= ~series.Format;
				foreach (IDataSeries s in BindableSeries) {
					if (s.Format == series.Format && s.IsVisible) {
						_Formats |= series.Format;
						break;
					}
				}
			}
			RefreshAxisVisibilities();
		}

		private void Clear()
		{
			Series.Clear();
		}

		private void Remove(int id)
		{
			try {
				var item = base.Series.First(i => ((i as LineSeries).DataContext as IDataSeries).Id == id);
				Series.Remove(item);
			} catch (InvalidOperationException) { }
		}

		/**
		 * Callback listens to changes to BindableSeries and updates Chart.Series property accordingly
		 * This implementation is a workaround because the base Chart.Series property is not a dependencyproperty
		 * and cannot be bound to. So we create a BindableSeries property which *can* be bound to,
		 * and then delegate changes to it to the underlying Series property
		 */
		private void SeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove ||
				e.Action == NotifyCollectionChangedAction.Replace) {
				_Formats = 0;
				for (int i = 0; i < e.OldItems.Count; i++) {
					Remove(((IDataSeries)e.OldItems[i]).Id);
				}
				var coll = (IEnumerable<IDataSeries>)sender;
				for (int i = 0; i < coll.Count(); i++) {
					_Formats |= coll.ElementAt(i).Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Add ||
				e.Action == NotifyCollectionChangedAction.Replace) {
				for (int i = 0; i < e.NewItems.Count; i++) {
					Plot((IDataSeries)e.NewItems[i]);
					_Formats |= ((IDataSeries)e.NewItems[i]).Format;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				Clear();
				_Formats = 0;
				if (((ObservableCollection<IDataSeries>)sender).Count > 0) {
					foreach (var item in ((ObservableCollection<IDataSeries>)sender)) {
						Plot(item);
						_Formats |= item.Format;
					}
				}
			}
			RefreshAxisVisibilities();
		}

		#region Cursor Stuff
		/**
		 * Cursor implementation is here. The cursor could and probably should be a discrete control,
		 * but to save time it is implemented here
		 */

		private Grid VerticalCursor;
		private Grid CursorCanvas;

		public string CursorLabel { get; set; }

		public bool ShowCursor { get; set; } = false;

		private DateTime GetDateAtCursorPosition(MouseEventArgs e)
		{
			double pos = e.GetPosition(CursorCanvas).X;
			var span = X.ActualMaximum - X.ActualMinimum;
			double ratio = pos / CursorCanvas.ActualWidth;
			var date = new DateTime((long)(span.Value.Ticks * ratio) + X.ActualMinimum.Value.Ticks);
			return date;
		}

		private TaskCompletionSource<DateTime> cursorPicked;
		private void MouseClickHandler(object sender, MouseButtonEventArgs e)
		{
			if (cursorPicked != null) {
				cursorPicked.TrySetResult(GetDateAtCursorPosition(e));
			}
		}

		private void MouseOverHandler(object sender, MouseEventArgs e)
		{
			double pos = e.GetPosition(CursorCanvas).X;

			VerticalCursor.Margin =
				new Thickness(pos - (VerticalCursor.ActualWidth / 2),0,0,0);

			
			CursorLabel = GetDateAtCursorPosition(e).ToString("HH:mm\ndd-MMM");
			NotifyPropertyChanged("CursorLabel");
		}

		/** 
		 * This method grabs the View objects from the template when the template is applied.
		 * The view objects are needed to move the views along with the mouse. Technically against
		 * MVVM to hold references to View in ViewModel, but its faster to implement here
		 */
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			VerticalCursor = GetTemplateChild("VerticalCursor") as Grid;
			CursorCanvas = GetTemplateChild("CursorCanvas") as Grid;
		}

		#endregion Cursor Stuff
	}
}
