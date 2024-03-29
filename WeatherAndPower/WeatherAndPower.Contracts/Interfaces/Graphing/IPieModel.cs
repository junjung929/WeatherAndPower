﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IPieModel
	{
		string Name { get; set; }
		ObservableCollection<DataPoint> Data { get; }
	}
}
