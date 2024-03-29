﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using static WeatherAndPower.Contracts.IAddWindowModel;

namespace WeatherAndPower.UI
{
    public class AddWindowViewModel : ViewModelBase
    {
        public AddWindow AddWindow { get; set; }

        private IAddWindowModel _Model;
        public IAddWindowModel Model
        {
            get { return _Model; }
            private set
            {
                if (_Model != value)
                {
                    _Model = value;
                }
            }
        }

        private InputViewModelBase _SelectedViewModel;
        private DataTypeEnum _DataType = (DataTypeEnum)0x01;

        public DataTypeEnum DataType
        {
            get { return _DataType; }
            set
            {
                _DataType = value; NotifyPropertyChanged("DataType");
                UpdateViewCommand.Execute(DataType);
            }
        }

        private void UpdateSelectedViewModel(DataTypeEnum dataType)
        {
            if (dataType == DataTypeEnum.Power)
            {
                var powerModel = Model.CreateNewPowerInputModel();
                SelectedViewModel = new PowerInputViewModel(powerModel);
            }
            else
            {
                var weatherModel = Model.CreateNewWeatherInputModel();
                SelectedViewModel = new WeatherInputViewModel(weatherModel);
            }
        }

        public InputViewModelBase SelectedViewModel
        {
            get { return _SelectedViewModel; }
            set
            {
                _SelectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ObservableCollection<DataTypeEnum> DataTypes { get; }
           = new ObservableCollection<DataTypeEnum>(
               Enum.GetValues(typeof(DataTypeEnum))
               .Cast<DataTypeEnum>());

        public RelayCommand UpdateViewCommand => new RelayCommand(() => UpdateSelectedViewModel(DataType));
        public RelayCommand AddGraphCommand => new RelayCommand(() =>
        {
            if (DataType == DataTypeEnum.Power)
            {
                var powerViewModel = (PowerInputViewModel)SelectedViewModel;
                var dateTimeViewModel = powerViewModel.DateTimeViewModel;
                try
                {
                    Model.AddPowerGraphAction(powerViewModel.SPowerType, dateTimeViewModel.StartTime, dateTimeViewModel.EndTime, PlotName);
                    AddWindow.Close();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
            else
            {
                try
                {
                    var weatherViewModel = (WeatherInputViewModel)SelectedViewModel;
                    var dateTimeViewModel = weatherViewModel.DateTimeViewModel;
                    if (weatherViewModel.SelectedWeatherTypes.Count < 1 && weatherViewModel.SelectedMedians.Count < 1)
                    {
                        throw new Exception("Please choose at least one weather type or one median parameter");
                    }
                    string parameters = String.Join(",", weatherViewModel.SelectedWeatherTypes.Concat(weatherViewModel.SelectedMedians));
                    Console.WriteLine("Params " + parameters);


                    Model.AddWeatherGraph(
                        weatherViewModel.SelectedCity.ToString(),
                        parameters,
                        dateTimeViewModel.StartTime,
                        dateTimeViewModel.EndTime,
                        PlotName,
                        weatherViewModel.SelectedParameter,
                        weatherViewModel.SelectedInterval.Value);
                    AddWindow.Close();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        });


        public string PlotName { get; set; }
        public AddWindowViewModel(IAddWindowModel model)
        {
            _Model = model;
            UpdateSelectedViewModel(DataType);
        }
    }
}
