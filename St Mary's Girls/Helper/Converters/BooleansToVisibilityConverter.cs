using System;
using System.Windows;
using System.Windows.Data;

namespace Helper.Converters
{
    public class BooleansToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
                return Binding.DoNothing;
            if (values.Length == 0)
                return Binding.DoNothing;
            bool visible = true;
            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    bool temp = (values == null) ? false : true;
                     temp = (values == DependencyProperty.UnsetValue) ? false : true;
                     if (!temp)
                         visible = false;
                     else
                         visible = visible && (bool)values[i];
                }
                catch { visible = visible && false; }
            }
            return (visible ? Visibility.Visible : Visibility.Collapsed);

            }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
