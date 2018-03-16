using System.IO;
using System.Windows.Media.Imaging;

namespace UmanyiSMS.Lib.Converters
{
    public static class ImageConverters
    {
        public static byte[] BitmapSourceToByteArray(BitmapSource source)
        {
            byte[] buff = null;
            try
            {
                PngBitmapEncoder en = new PngBitmapEncoder();

                using (MemoryStream stream = new MemoryStream())
                {
                    en.Frames.Add(BitmapFrame.Create(source));
                    en.Save(stream);
                    buff = stream.ToArray();
                }
            }
            catch { }
            return buff;
        }
    }
}
