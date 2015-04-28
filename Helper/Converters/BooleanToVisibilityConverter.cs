using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Helper.Converters
{
    public class BooleanToVisibilityConverter
        : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool) {
                flag = (bool)value;
            }
            else if (value is bool?) {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }

            bool inverse = (parameter as string) == "inverse";

            if (inverse) {
                return (flag ? Visibility.Collapsed : Visibility.Visible);
            }
            else {
                return (flag ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
