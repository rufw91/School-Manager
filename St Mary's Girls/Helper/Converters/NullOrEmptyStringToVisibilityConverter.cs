using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Helper.Converters
{
   
    public class NullOrEmptyStringToVisibilityConverter
        : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = value == null;
            if (value is string) {
                flag = string.IsNullOrEmpty((string)value);
            }
            var inverse = (parameter as string) == "inverse";

            if (inverse) {
                return (flag ? Visibility.Collapsed : Visibility.Visible);
            }
            else {
                return (flag ? Visibility.Visible : Visibility.Collapsed);
            }
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
