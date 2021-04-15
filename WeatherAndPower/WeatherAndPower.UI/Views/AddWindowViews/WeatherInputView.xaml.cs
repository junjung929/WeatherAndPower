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
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    /// <summary>
    /// Interaction logic for WeatherInputView.xaml
    /// </summary>
    public partial class WeatherInputView : UserControl
    {
        public WeatherInputView()
        {
            InitializeComponent();
        }

        private void WeatherTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (WeatherInputViewModel)DataContext;
            viewModel.SelectedWeatherTypes = WeatherTypeListBox.SelectedItems.OfType<WeatherType>().ToList();
        }

        private void MedianListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (WeatherInputViewModel)DataContext;
            viewModel.SelectedMedians = MedianListBox.SelectedItems.OfType<WeatherType>().ToList();
        }
    }
}
