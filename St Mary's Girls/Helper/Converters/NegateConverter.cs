using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Helper.Converters
{
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool selIndex;
            try
            {
                
                if ((value != null) && (value != DependencyProperty.UnsetValue) && (bool.TryParse(value.ToString(), out selIndex)))
                {

                    return !selIndex;
                }
                else return false;
            }
            catch { return false; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool selIndex;
            try
            {

                if ((value != null) && (value != DependencyProperty.UnsetValue) && (bool.TryParse(value.ToString(), out selIndex)))
                {

                    return !selIndex;
                }
                else return false;
            }
            catch { return false; }
        }
    }
}
