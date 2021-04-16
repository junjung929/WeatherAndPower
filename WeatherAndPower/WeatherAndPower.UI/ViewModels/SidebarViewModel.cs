using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    public class SidebarViewModel : ViewModelBase
    {
        public ISidebarModel Model { get; private set; }

        public ObservableCollection<IDataSeries> Data
        {
            get
            {
                return Model.Data;
            }
        }

        public IDataSeries SelectedSeries { get; set; }

        public RelayCommand ClearGraphCommand => new RelayCommand(() => Model.ClearGraph());
        public RelayCommand OpenDataCommand => new RelayCommand(() => Model.OpenData("test.json"));
        public RelayCommand SaveDataCommand => new RelayCommand(() =>
        {
            try
            {
                Model.SaveSelectedData("test.json");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        });
        public RelayCommand SaveDataImageCommand => new RelayCommand(() => Model.SaveDataImage("test.png"));
        public RelayCommand AddDataCommand => new RelayCommand(() => Model.AddData());
        public RelayCommand CompareDataCommand => new RelayCommand(() =>
        {
            MessageBox.Show("Select a time line in the graph for pie chart to compare");
            Model.CompareData();
        });
        public RelayCommand RemoveDataCommand => new RelayCommand(() =>
        {
            try
            {
                Model.RemoveSelectedData();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        });

        public SidebarViewModel(ISidebarModel model)
        {
            Model = model;
        }
    }
}
