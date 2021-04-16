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
using DataFormat = WeatherAndPower.Contracts.DataFormat;
using System.Windows.Input;
using System.Windows.Controls;

namespace WeatherAndPower.UI
{

	public class DataPlotViewModel : ViewModelBase
	{

		#region Properties

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

		//private DataFormat _Formats = 0;

		public ObservableCollection<IDataSeries> Data 
		{
			get { return Model.Data; }
		}

		private CustomChart _Chart { get; set; }

		#endregion

		private void Clear() { Data.Clear(); }

		private void Remove(int id) {
			try {
				var item = Data.First(i => i.Id == id);
				Data.Remove(item);
			} catch (InvalidOperationException) { }
		}

		public DataPlotViewModel(IDataPlotModel model, FrameworkElement view)
		{
			Model = model;
			_Chart = (CustomChart)view.FindName("theChart");
			_Chart.SeriesClicked += SeriesClicked;
		}

		private void SeriesClicked(object sender, MouseButtonEventArgs e)
		{
			var series = (sender as CustomLineSeries).DataContext as IDataSeries;
			var id = series.Id;
			if (!Keyboard.IsKeyDown(Key.LeftCtrl)) {
				foreach (var plot in Data) {
					plot.IsSelected = false;
				}
			}
			series.IsSelected = true;
			//Model.SaveChartJson("test.json", id);
		}
	}
}
