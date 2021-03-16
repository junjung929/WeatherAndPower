﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class DataSeries
	{
		public int Id { get; set; } = 0;

		public string Name { get; set; }

		public byte[] Color { get; } = { 0, 0, 0 };

		public DataFormat Format { get; set; }

		//public DateTime Start { get; set; }

		//public DateTime End { get; set; }

		//public double[] Data { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }
		
		//public long[] Ticks {
		//	get {
		//		var startTicks = Start.Ticks;
		//		var ticks = End.Ticks - startTicks;
		//		var increment = ticks / Data.Length;
		//		return Enumerable.Range(1, Data.Length).Select(i => i * increment).ToArray();
		//	}
		//}

		//public DataSeries(string name, DataFormat format, DateTime start, DateTime end, double[] data)
		//{
		//	Name = name; Data = data; Start = start; End = end; Format = format;
		//}

		public void RandomizeColor()
		{
			var rand = new Random();
			Color[0] = (byte)rand.Next(0, 255);
			Color[1] = (byte)rand.Next(0, 255);
			Color[2] = (byte)rand.Next(0, 255);
		}
		public DataSeries(string name, DataFormat format, List<Tuple<DateTime, IData>> series)
		{
			Name = name;
			Format = format;
			Series = series;
			RandomizeColor();
		}
	}
}
