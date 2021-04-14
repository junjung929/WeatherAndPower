using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class PieFactory : IWindowFactory
	{
		public void CreateWindow(IPieModel model)
		{
			var PieViewModel = new PieViewModel(model);
			var window = new PieWindow();
			window.DataContext = PieViewModel;
			window.Show();
		}
	}
}
