using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

using xigt;

namespace xigt_editor
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public sealed class MaxConverter : IMultiValueConverter
	{
		public static readonly MaxConverter Instance;
		static MaxConverter() { Instance = new MaxConverter(); }
		MaxConverter() { }

		public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
		{
			var dmax = Double.MinValue;
			for (int i = 0; i < values.Length; i++)
			{
				var d = (Double)values[i];
				if (d > dmax)
					dmax = d;
			}
			return dmax;
		}

		public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	};
}