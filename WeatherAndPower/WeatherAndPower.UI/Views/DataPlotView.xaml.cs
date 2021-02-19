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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Controls.DataVisualization.Charting;

namespace WeatherAndPower.UI
{
	/// <summary>
	/// Interaction logic for DataPlotView.xaml
	/// </summary>
	public partial class DataPlotView : UserControl
	{
		public DataPlotView()
		{
			new Chart(); // WPF cant find the assembly if we dont call it before InitializeComponent for some reason.
			InitializeComponent();
		}

	}
}
