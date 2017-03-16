using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace UmanyiSMS.Lib.Converters
{
   
    public class InverseVisibilityConverter
        : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (Visibility)value;
                return (flag==Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
            
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (Visibility)value;
            return (flag == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }
    }
}
