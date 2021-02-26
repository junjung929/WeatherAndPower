using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAndPower.Core;
using WeatherAndPower.Data;
using WeatherAndPower.UI;

namespace WeatherAndPower
{
	public static class Program
	{
		public static Window MainWindow;

		[STAThread]
		static void Main(string[] args)
		{
			MainWindow?.Close();
			AssembleMainWindow();
		}
		private static void AssembleMainWindow()
		{
			MainWindow = new UI.MainWindow();

			//Initialize all the various modules here

			var dataPlotView = ((FrameworkElement)MainWindow.FindName("DataPlot"));
			var dataPlotModel = new DataPlotModel();
			var dataPlotViewModel = new DataPlotViewModel(dataPlotModel, dataPlotView);
			dataPlotView.DataContext = dataPlotViewModel;

			var placeholderModel = new PlaceholderModel(dataPlotModel);
			var placeholderViewModel = new PlaceholderViewModel(placeholderModel);
			var placeholderView = ((FrameworkElement)MainWindow.FindName("PlaceholderModule"));
			placeholderView.DataContext = placeholderViewModel;

			MainWindow.ShowDialog();
		}
	}
}
