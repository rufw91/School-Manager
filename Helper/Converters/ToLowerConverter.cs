using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;

namespace Helper.Converters
{
    public class ToLowerConverter
        : IValueConverter
    {
        
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) {
                var strValue = value.ToString();

                
                return strValue.ToLowerInvariant();
            }
            return null;
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
