using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
	class PieModel : IPieModel
	{
		public string Name { get; set; }
		public ObservableCollection<DataPoint> Data { get; } = new ObservableCollection<DataPoint>();

		public PieModel(string name)
		{
			Name = name;
		}
	}
}
