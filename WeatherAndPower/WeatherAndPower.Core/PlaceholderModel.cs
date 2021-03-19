using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;

namespace WeatherAndPower.Core
{
    public class PlaceholderModel : AbstractModel, IPlaceholderModel
    {

		private DataPlotModel _DataPlot;
		public DataPlotModel DataPlot
		{
			get { return _DataPlot; }
			private set {
				if (_DataPlot != value) {
					_DataPlot = value;
				}
			}
		}
		private int _PlaceholderProperty = 0;
        public int PlaceholderProperty
		{
			get {
				return _PlaceholderProperty;
			}
			private set {
				if (_PlaceholderProperty != value) {
					_PlaceholderProperty = value;
				}
			}
		}

		private string _DataName;
		public string DataName {
			get { return _DataName; }
			set {
				if (_DataName != value) {
					_DataName = value;
					NotifyPropertyChanged("DataName");
				}
			}
		}

		public void PlaceholderAction1()
		{
			DataPlot.AddRandomPlot(DataName);

		}

		public void PlaceholderAction2()
		{
			DataPlot.Clear();
		}

		public  void PlaceholderAction3()
		{
			DataPlot.Remove(DataName);
		}

		public void PlaceholderAction5()
		{
			var seires = Task.Run(() => Fingrid.Get(
				PowerType.WindPowerProdRT,
				new DateTime(2021, 3, 14, 2, 00, 00),
				new DateTime(2021, 3, 14, 5, 00, 00)))
				.Result;
			DataPlot.Data.Add(seires);
		}

        public void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string PlotName)
        {
			var series = Task.Run(() => Fingrid.Get(powerType, startTime, endTime)).Result;
			series.Name = PlotName;
			DataPlot.Data.Add(series);
        }

        public PlaceholderModel(DataPlotModel dataPlot)
		{
			DataPlot = dataPlot;
		}
		

		
	}
}
