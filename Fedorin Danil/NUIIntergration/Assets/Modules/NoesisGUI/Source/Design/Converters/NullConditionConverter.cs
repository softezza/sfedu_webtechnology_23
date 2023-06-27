#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using UnityEngine;
#else
using System.Windows.Data;
#endif

using System;
using System.Globalization;

namespace SmartTwin.NoesisGUI.Converters
{
	/// <summary>
	/// Конвертер проверки на Null.
	/// Возвращает True, если объект не равен null.
	/// </summary>
	public class NullConditionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
		}
	}
}
