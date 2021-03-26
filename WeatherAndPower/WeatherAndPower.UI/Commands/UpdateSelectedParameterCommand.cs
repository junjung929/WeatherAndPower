using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.ViewModels.AddWindow;

namespace WeatherAndPower.UI.Commands
{
    public class UpdateSelectedParameterCommand : ICommand
    {
        private WeatherInputViewModel viewModel;

        public UpdateSelectedParameterCommand(WeatherInputViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine(parameter);
            var checkBox = (System.Windows.Controls.CheckBox)parameter;
            Console.WriteLine(checkBox.DataContext);
            var weatherType = (WeatherType)checkBox.DataContext;


            List<WeatherType> selectedParameterTypes = viewModel.SelectedParameters;

            if (selectedParameterTypes.Contains(weatherType))
            {
                selectedParameterTypes.Remove(weatherType);
            }
            else
            {
                var parameterTypes = selectedParameterTypes.ToList().Select(x => x.ParameterType).Distinct();

                // Make sure only either forecast or observation can be fetched at a time
                if ((weatherType.ParameterType == WeatherType.ParameterEnum.Forecast
                    && !parameterTypes.Contains(WeatherType.ParameterEnum.Observation))
                    || (weatherType.ParameterType == WeatherType.ParameterEnum.Observation
                    && !parameterTypes.Contains(WeatherType.ParameterEnum.Forecast))
                    || (weatherType.ParameterType == WeatherType.ParameterEnum.Median))
                {
                    selectedParameterTypes.Add(weatherType);
                    viewModel.SelectedParameterType = weatherType.ParameterType;
                    return;
                }
                System.Windows.MessageBox.Show("Please choose parameters of either observation or forecast");
                checkBox.IsChecked = false;
            }
        }
    }
}
