using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Helper.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color flag;
            if (value is Color)
            {
                flag = (Color)value;
                return new SolidColorBrush(flag);
            }
            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
