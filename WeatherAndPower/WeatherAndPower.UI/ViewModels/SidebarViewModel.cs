using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class SidebarViewModel : ViewModelBase
	{
		public ISidebarModel Model { get; set; }

        public RelayCommand ClearGraphCommand => new RelayCommand(() => Model.ClearGraph());
        public RelayCommand OpenDataCommand => new RelayCommand(() => Model.OpenData("test.json"));
        public RelayCommand SaveDataCommand => new RelayCommand(() => Model.SaveData("test.json"));
        public RelayCommand AddDataCommand => new RelayCommand(() => );
        public RelayCommand CompareDataCommand => new RelayCommand(() => Model.CompareData());
        public RelayCommand RemoveDataCommand => new RelayCommand(() => Model.RemoveData());
	}
}
