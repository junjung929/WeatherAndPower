using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    class AddWindowModel : AbstractModel, IAddWindowModel
    {
        private DataFormat _dataType = DataFormat.Power;

        public DataFormat DataType
        {
            get { return _dataType; }
            set { _dataType = value; NotifyPropertyChanged("DataType"); }
        }



        public void AddButtonAction()
        {
            throw new NotImplementedException();
        }

        public void RadioButtonAction()
        {
            throw new NotImplementedException();
        }
    }
}
