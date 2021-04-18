using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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


        //private IPreference _Preference { get; set; }
        public IPreference Preference
        {
            get { return Model.Preference; }
            //set
            //{
            //    _Preference = value;
            //    NotifyPropertyChanged("Preference");
            //}
        }
        //private DataTypeEnum _DataType = (DataTypeEnum)0x01;

        public DataTypeEnum DataType
        {
            get { return Model.Preference.DataType; }
            set
            {
                Model.Preference.DataType = value;
                NotifyPropertyChanged("DataType");
                UpdateViewCommand.Execute(DataType);
            }
        }



        private InputViewModelBase _SelectedViewModel;
        public InputViewModelBase SelectedViewModel
        {
            get { return _SelectedViewModel; }
            set
            {
                if (_SelectedViewModel != value)
                {
                    _SelectedViewModel = value;
                    NotifyPropertyChanged(nameof(SelectedViewModel));
                }
            }
        }
        public ObservableCollection<DataTypeEnum> DataTypes { get; }
           = new ObservableCollection<DataTypeEnum>(
               Enum.GetValues(typeof(DataTypeEnum))
               .Cast<DataTypeEnum>());
        public string PlotName
        {
            get
            {
                return Model.Preference.PlotName;
            }
            set
            {
                Model.Preference.PlotName = value;
                NotifyPropertyChanged("PlotName");
            }
        }

        private void UpdateSelectedViewModel(DataTypeEnum dataType)
        {
            if (dataType == DataTypeEnum.Power)
            {
                var preference = Model.CreateNewPowerPreference();
                Model.Preference = preference;
                var powerModel = Model.CreateNewPowerInputModel(Preference as IPowerPreference);
                SelectedViewModel = new PowerInputViewModel(powerModel);
            }
            else
            {
                var preference = Model.CreateNewWeatherPreference();
                Model.Preference = preference;
                var weatherModel = Model.CreateNewWeatherInputModel(Preference as IWeatherPreference);
                SelectedViewModel = new WeatherInputViewModel(weatherModel);
            }
        }

        private void InitializeComponent()
        {
            //Preference = Model.CreateNewPowerPreference();
            UpdateSelectedViewModel(Model.Preference.DataType);
            //DataType = Preference.DataType; 
            //PlotName = Preference.PlotName;
        }

        public RelayCommand UpdateViewCommand => new RelayCommand(() => UpdateSelectedViewModel(DataType));
        public RelayCommand AddGraphCommand => new RelayCommand(() =>
        {
            if (DataType == DataTypeEnum.Power)
            {
                try
                {
                    Model.AddPowerGraphAction();
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
                    Model.AddWeatherGraph();
                    AddWindow.Close();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        });

        public RelayCommand OpenPreferenceCommand => new RelayCommand(() =>
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    Model.OpenPreference(openDialog.FileName);
                    NotifyPropertyChanged("DataType");
                    var powerModel = Model.CreateNewPowerInputModel(Model.Preference as IPowerPreference);
                    SelectedViewModel = new PowerInputViewModel(powerModel);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        });
        public RelayCommand SavePreferenceCommand => new RelayCommand(() =>
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    Model.SavePreference(saveDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        });


        public AddWindowViewModel(IAddWindowModel model)
        {
            _Model = model;
            InitializeComponent();
        }
    }
}
