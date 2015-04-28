using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Helper.Converters
{
    public class ValidationErrorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            if (value is string)
                return (string)value;

            if (value is List<String>)
            {
                string temp = "";
                var t=(List<string>)value;
                foreach(var f in t )
                    temp += " - " + f + "\r\n"; 
                return temp;
            }
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
