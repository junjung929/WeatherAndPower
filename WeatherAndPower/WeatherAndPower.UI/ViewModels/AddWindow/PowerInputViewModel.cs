using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Commands;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    public class PowerInputViewModel : ViewModelBase
    {
        MainViewModel ParentViewModel { get; set; }

        public ObservableCollection<PowerType> PowerTypes { get; set; }
        private PowerType selectedPowerType { get; set; }
        public PowerType SPowerType
        {
            get
            {
                return selectedPowerType;
            }
            set
            {
                Console.WriteLine(value);
                selectedPowerType = value;
                NotifyPropertyChanged("SPowerType");

                //ParentViewModel.IsStartTimePickerEnabled = true;
                //ParentViewModel.IsEndTimePickerEnabled = true;
                ParentViewModel.SetDateTimeMinMaxToDefault();

                if (value.Equals(PowerType.EConsumForecast24H)
                    || value.Equals(PowerType.EProdPrediction24H))
                {
                    ParentViewModel.UpdateDateTimeCommand.Execute("n24h");
                    ParentViewModel.DateTimeMax = ParentViewModel.EndTime;
                }
                else if (value.Equals(PowerType.SolarPowerGenerationForecast)
                    || value.Equals(PowerType.WindPowerGenerationForecast))
                {
                    ParentViewModel.UpdateDateTimeCommand.Execute("n36h");
                    ParentViewModel.DateTimeMax = ParentViewModel.EndTime;
                }
                if (value.ParameterType.Equals(PowerType.ParameterEnum.Observation)
                    || value.ParameterType.Equals(PowerType.ParameterEnum.RealTime))
                {
                    DateTime currentHour = DateTime.Today.AddHours(DateTime.Now.Hour);
                    if (DateTime.Compare(ParentViewModel.StartTime, currentHour) > 0)
                    {
                        ParentViewModel.StartTime = currentHour.AddHours(-1);
                    }
                    ParentViewModel.EndTime = currentHour.AddTicks(-1);
                    ParentViewModel.DateTimeMax = currentHour.AddTicks(-1);
                }
            }
        }

        public PowerInputViewModel(MainViewModel viewModel)
        {
            ParentViewModel = viewModel;
            PowerTypes = new ObservableCollection<PowerType>(PowerType.GetAll<PowerType>());
        }
    }
}
