using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherAndPower.Contracts;
using WeatherAndPower.Core;
using WeatherAndPower.UI;

namespace WeatherAndPower
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public void Application_Startup(object sender, StartupEventArgs e)
		{
			InitializeComponent();
			MainWindow?.Close();
			AssembleMainWindow();
		}

		private void AssembleMainWindow()
		{
			MainWindow = new MainWindow();

			//Initialize all the various modules here

			var dataPlotView = (FrameworkElement)MainWindow.FindName("DataPlot");
			var chartView = (ICustomChart)dataPlotView.FindName("theChart");
			var dataPlotModel = new DataPlotModel(chartView);
			//var dataPlotModel = new DataPlotModel();
			var dataPlotViewModel = new DataPlotViewModel(dataPlotModel, dataPlotView);
			dataPlotView.DataContext = dataPlotViewModel;

			var windowFactory = new PieFactory();

			var placeholderModel = new PlaceholderModel(dataPlotModel, windowFactory);
			var placeholderViewModel = new PlaceholderViewModel(placeholderModel);
			var placeholderView = ((FrameworkElement)MainWindow.FindName("PlaceholderModule"));
			placeholderView.DataContext = placeholderViewModel;

			MainWindow.Show();
		}
	}
}
