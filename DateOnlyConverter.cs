using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoFix
{
    public class DateOnlyConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return ((DateOnly)value).ToDateTime(new TimeOnly());
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return DateOnly.FromDateTime((DateTime)value);
        }
    }
}
