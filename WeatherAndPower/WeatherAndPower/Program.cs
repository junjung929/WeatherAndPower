using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAndPower.Core;
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
			var placeholderModel = new PlaceholderModel();
			var placeholderViewModel = new PlaceholderViewModel(placeholderModel);
			var placeholderView = ((FrameworkElement)MainWindow.FindName("PlaceholderModule"));
			if (placeholderView != null) placeholderView.DataContext = placeholderViewModel;
			else throw new NullReferenceException("Error");

			MainWindow.ShowDialog();
		}
	}
}
