﻿using System;
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
	/// </summary>
	[TemplatePart(Name = "VerticalCursor", Type = typeof(Grid))]
	[TemplatePart(Name = "CursorCanvas", Type = typeof(Grid))]
	public partial class CustomChart : Chart, ICustomChart, INotifyPropertyChanged
	{
		private DataFormat _Formats = 0;

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

		private void RefreshAxisVisibilities()
		{
			NotifyPropertyChanged("ShowTemperatureAxis");
			NotifyPropertyChanged("ShowPowerAxis");
			NotifyPropertyChanged("ShowCloudinessAxis");
			NotifyPropertyChanged("ShowWindAxis");
			NotifyPropertyChanged("ShowHumidityAxis");
			NotifyPropertyChanged("ShowPrecipitationAxis");
		}

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

		private Shape _CreateTestShape()
		{
			return new Ellipse()
			{
				Height = 200,
				Width = 200,
				StrokeThickness = 4,
				Stroke = new SolidColorBrush() { Color = Colors.Blue },
				Fill = new SolidColorBrush() { Color = Colors.Red }
			};
		}

		public List<Tuple<int, int>> Line { get; set; } = new List<Tuple<int, int>>();




		public CustomChart()
		{
			//Axes.Add(X);
			//Axes.Add(TemperatureAxis);
			//Axes.Add(PowerAxis);
			//Axes.Add(CloudinessAxis);
			//Axes.Add(WindAxis);

			Line.Add(new Tuple<int, int>(1, 2));
			Line.Add(new Tuple<int, int>(2, 3));

			//ShowLegend = false;

			InitializeComponent();
		}


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

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			VerticalCursor = GetTemplateChild("VerticalCursor") as Grid;
			CursorCanvas = GetTemplateChild("CursorCanvas") as Grid;
		}

		#endregion Cursor Stuff
	}
}