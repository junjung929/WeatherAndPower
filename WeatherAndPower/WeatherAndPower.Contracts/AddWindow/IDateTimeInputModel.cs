﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IDateTimeInputModel
    {
        Enumeration EDateTimeRange { get; }
        Tuple<DateTime, DateTime> GetNewDateTimeRange(string dateTimeRange);
    }
}