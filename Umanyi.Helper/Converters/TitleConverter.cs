using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;

namespace UmanyiSMS.Lib.Converters
{
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var strValue = value.ToString();
                strValue += " Management System";
                return strValue;
            }
            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}


