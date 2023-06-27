#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Data;
#endif

using System;
using System.Globalization;
using SmartTwin.NoesisGUI.Tools;

namespace SmartTwin.NoesisGUI.Converters
{
	/// <summary>
	/// Конвертер логического флага в <see cref="Visibility"/> и наоборот
	/// </summary>
	public class VisiblilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var mode = Visibility.Hidden;

			if (parameter is Visibility newMode)
				mode = newMode;

			if (value is bool isVisible)
				return isVisible ? Visibility.Visible : mode;

			return mode;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
				return visibility == Visibility.Visible;

			return false;
		}
	}
}
