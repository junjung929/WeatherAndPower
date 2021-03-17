using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IAddWindowModel
    {
        DataFormat DataType { get; set; }

        void AddButtonAction();

        void RadioButtonAction();
    }
}
