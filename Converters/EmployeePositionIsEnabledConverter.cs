using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AutoFix
{
    public class EmployeePositionIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;

            var positions = ((string)parameter)?.Split(",").Select(p => EmployeePosition.InternalToDisplayName.GetValueOrDefault(p)!).ToArray() ?? Array.Empty<string>();
            return ((Employee)value).HasPosition(positions);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
