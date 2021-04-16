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
    /// Interaction logic for DateTimeInputView.xaml
    /// </summary>
    public partial class DateTimeInputView : UserControl
    {
        public DateTimeInputView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var button = sender as Button;
            var selected = button.Tag as IDateTimeRange;

            //((DateTimeViewModel)DataContext).SelectedDateTimeRange = selected;
            ((DateTimeViewModel)DataContext).UpdateDateTimes(selected);

        }
    }
}
