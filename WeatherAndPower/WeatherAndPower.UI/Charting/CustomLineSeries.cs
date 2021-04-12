using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeatherAndPower.UI
{
	public class CustomLineSeries : LineSeries
	{
		private Style _DefaultStyle;
		private Style _HoverStyle;
		private void onMouseEnter(object sender, MouseEventArgs e)
		{
			PolylineStyle = _HoverStyle;
		}

		private void onMouseLeave(object sender, MouseEventArgs e)
		{
			PolylineStyle = _DefaultStyle;
		}

		private Tuple<Color, Color> gradientFromColor(Color color)
		{
			int w = 50; // Gradient width
			Color col1 = Color.FromRgb(
				(byte)Math.Min(color.R+w,255),
				(byte)Math.Min(color.G+w,255),
				(byte)Math.Min(color.B+w,255));
			Color col2 = Color.FromRgb(
				(byte)Math.Max(color.R-w,0),
				(byte)Math.Max(color.G-w,0),
				(byte)Math.Max(color.B-w,0));

			return new Tuple<Color, Color>(col1, col2);
		}

		public Color LineColor {
			set {
				var gradient = new LinearGradientBrush() {
					EndPoint = new Point(0, 0),
					StartPoint = new Point(0, 1)
				};
				var colors = gradientFromColor(value);
				gradient.GradientStops.Add(new GradientStop(colors.Item1, 0));
				gradient.GradientStops.Add(new GradientStop(colors.Item2, 1));

				var colorSetter = new Setter()
				{
					Property = LineDataPoint.BackgroundProperty,
					Value = gradient
				};
				var opacitySetter = new Setter()
				{
					Property = LineDataPoint.OpacityProperty,
					Value = 0.0
				};

				Style style = new Style(typeof(DataPoint));
				style.Setters.Add(colorSetter);
				style.Setters.Add(opacitySetter);
				DataPointStyle = style;
			}
		}

		public CustomLineSeries()
		{
			this.MouseEnter += onMouseEnter;
			this.MouseLeave += onMouseLeave;

			Style lineStyle = new Style(typeof(Polyline));
			lineStyle.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 2.0));
			lineStyle.Setters.Add(new Setter(Polyline.StrokeLineJoinProperty, PenLineJoin.Round));
			PolylineStyle = lineStyle;

			Style hoverStyle = new Style(typeof(Polyline));
			hoverStyle.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 3.0));
			hoverStyle.Setters.Add(new Setter(Polyline.StrokeLineJoinProperty, PenLineJoin.Round));

			_DefaultStyle = lineStyle;
			_HoverStyle = hoverStyle;
		}
	}
}
