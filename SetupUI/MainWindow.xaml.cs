using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetupUI
{
    public partial class SetupUIView : CustomWindow
    {
        private SetupUIViewModel viewModel;

        public SetupUIView()
        {
            InitializeComponent();
        }

        public static ImageSource GetIcon(string strPath, bool bSmall)
        {
            NativeMethods.SHSTOCKICONINFO iconResult = new NativeMethods.SHSTOCKICONINFO();
            iconResult.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(iconResult);

            NativeMethods.SHGetStockIconInfo(
                NativeMethods.SHSTOCKICONID.SIID_SHIELD,
                NativeMethods.SHGSI.SHGSI_ICON | NativeMethods.SHGSI.SHGSI_SMALLICON,
                ref iconResult);
            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                        iconResult.hIcon,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DestroyIcon(iconResult.hIcon);
            return img;
        }

        private void SetUacShield()
        {
            
        }

        private void RunAsAdmin(string fileName, string args)
        {
            var processInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = fileName,
                Arguments = args,
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Win32Exception)
            {
                // Do nothing...
            }
        }


    }
}
