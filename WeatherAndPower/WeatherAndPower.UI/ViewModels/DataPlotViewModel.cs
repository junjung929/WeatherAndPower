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

		private void Clear()
		{
			_Chart.Series.Clear();
		}

		private void Remove(int id)
		{
			var item = _Chart.Series.First(i => i.GetHashCode() == id);
			_Chart.Series.Remove(item);
		}

		#region Min Max Properties
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

		private int? _YMin = null;
		public int? YMin
		{
			get { return _YMin; }
			set {
				if (_YMin != value) {
					_YMin = value;
					NotifyPropertyChanged("YMin");
				}
			}
		}

		private int? _YMax = null;
		public int? YMax
		{
			get { return _YMax; }
			set {
				if (_YMax != value) {
					_YMax = value;
					NotifyPropertyChanged("YMax");
				}
			}
		}
		#endregion

		public DataPlotViewModel(IDataPlotModel model, FrameworkElement view)
		{
			Model = model;
			_Chart = (Chart)view.FindName("theChart");
			Data.CollectionChanged += DataChanged;
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
					Plot<LineSeries>((DataSeries)e.NewItems[i]);
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset) {
				Clear();
				if (((ObservableCollection<DataSeries>)sender).Count > 0) {
					foreach (var item in ((ObservableCollection<DataSeries>)sender)) {
						Plot<LineSeries>(item);
					}
				}
			}
		}
	}
}
