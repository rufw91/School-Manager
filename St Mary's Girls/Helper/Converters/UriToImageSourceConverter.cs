using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Helper.Converters
{
    public class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            try
            {
                if (value != null)
                {

                    Uri newB = (Uri)value;
                    BitmapImage gy = new BitmapImage(newB);
                    return gy;
                }
                else
                    return DependencyProperty.UnsetValue;
            }
            catch { return DependencyProperty.UnsetValue; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
