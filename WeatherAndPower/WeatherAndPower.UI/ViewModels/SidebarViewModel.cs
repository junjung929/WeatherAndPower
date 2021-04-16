﻿using Microsoft.Win32;
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
        public RelayCommand OpenDataCommand => new RelayCommand(() => {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
            if (openDialog.ShowDialog() == true) {
                Model.OpenData(openDialog.FileName);
			}
        });
        public RelayCommand SaveDataCommand => new RelayCommand(() =>
        {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
                if (saveDialog.ShowDialog() == true) {
					try
					{
                        Model.SaveSelectedData(saveDialog.FileName);
					}
					catch (Exception e)
					{
						MessageBox.Show(e.Message);
					}
                }
        });
        public RelayCommand SaveDataImageCommand => new RelayCommand(() => Model.SaveDataImage("test.png"));
        public RelayCommand AddDataCommand => new RelayCommand(() => Model.AddData());
        public RelayCommand CompareDataCommand => new RelayCommand(() => Model.CompareData());
        public RelayCommand RemoveDataCommand => new RelayCommand(() => Model.RemoveSelectedData());

        public SidebarViewModel(ISidebarModel model)
        {
            Model = model;
        }
    }
}
