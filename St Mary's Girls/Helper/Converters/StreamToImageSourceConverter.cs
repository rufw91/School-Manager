using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Helper.Converters
{
    public class StreamToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            try
            {
                if (value != null)
                {                    
                    byte[] newB = (byte[])value;
                    BitmapImage gy = new BitmapImage();
                    using (MemoryStream mem = new MemoryStream(newB))
                    {
                        mem.Seek(0, System.IO.SeekOrigin.Begin);                        
                        gy.BeginInit();
                        gy.CacheOption = BitmapCacheOption.OnLoad;
                        if (parameter != null)
                        {
                            int width;
                            if (int.TryParse(parameter.ToString(), out width))
                                gy.DecodePixelWidth = width;
                        }
                        gy.StreamSource = mem;
                        gy.EndInit();
                    }
                    return gy;
                }
                else
                    return DependencyProperty.UnsetValue;
            }
            catch { return DependencyProperty.UnsetValue; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {

                    BitmapSource image = (BitmapSource)value;
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        return ms.ToArray();
                    }
                }
                else
                    return DependencyProperty.UnsetValue;
            }
            catch { return DependencyProperty.UnsetValue; }
        }
    }
}
