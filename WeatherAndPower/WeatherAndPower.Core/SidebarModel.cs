using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;

namespace WeatherAndPower.Core
{
	public class SidebarModel : AbstractModel, ISidebarModel
	{
        public DataPlotModel DataPlot { get; set; }

        public IWindowFactory WindowFactory { get; set; }

        private int _PlaceholderProperty = 0;
        public int PlaceholderProperty
        {
            get
            {
                return _PlaceholderProperty;
            }
            private set
            {
                if (_PlaceholderProperty != value)
                {
                    _PlaceholderProperty = value;
                }
            }
        }
		public ObservableCollection<IDataSeries> Data
		{
			get {
				return DataPlot.Data;
			}
		}

		public void ClearGraph()
		{
			DataPlot.Clear();
		}

		public async void CompareData()
		{
            DateTime point = await DataPlot.Pick();
            var data = DataPlot.Data.Where(d => d.IsComparable && d.Maximum > point && d.Minimum < point)
				.Select(d => d.GetDataPoint(point));

            if (data.Count() > 0) {
                string format = "HH:mm:ss on ddd dd'/'MM'/'yyyy";
				var model = new PieModel("Power production comparison at " + point.ToString(format, CultureInfo.CreateSpecificCulture("en-US")));
				foreach (var d in data) {
					model.Data.Add(d);
				}
				WindowFactory.CreateWindow(model);
			}
		}

        public void AddData()
		{
            WindowFactory.CreateWindow(new AddWindowModel(DataPlot));
		}

		public void OpenData(string path)
		{
			DataPlot.LoadChartJson(path);
		}

		public void RemoveSelectedData()
		{
            var ids = DataPlot.Data.Where(e => e.IsSelected).Select(e => e.Id).ToArray();
            if (ids.ToList().Count <1)
            {
                throw new Exception("Please select at least one data from the list");
            }
            foreach (var id in ids) {
                DataPlot.Remove(id);
			}
		}

        public void SaveSelectedData(string path)
		{
            var ids = DataPlot.Data.Where(e => e.IsSelected).Select(e => e.Id).ToArray();
            if (ids.Count() > 0) {
                DataPlot.SaveChartJson(path, ids);
            }
            else
            {
                throw new Exception("Please select at least one data from the list");
            }
		}

		public void SaveData(string path, params int[] ids)
		{
			DataPlot.SaveChartJson(path, ids);
		}

		public void SaveDataImage(string path)
		{
			DataPlot.SaveChartImage(path);
		}

		public SidebarModel(DataPlotModel dataPlot, IWindowFactory windowFactory)
		{
			DataPlot = dataPlot;
			WindowFactory = windowFactory;
		}
	}
}
