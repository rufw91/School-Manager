using Helper.Controls;
using UmanyiSMS.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class LocalTutorials : UserControl
    {
        public LocalTutorials()
        {
            InitializeComponent();
            media.Source = new Uri(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video1.mp4");
            this.DataContextChanged += (o, e) =>
                {
                    LocalTutorialsVM dt = DataContext as LocalTutorialsVM;
                    if (dt == null)
                        return;
                    dt.PropertyChanged += (o1, e1) =>
                        {
                            if (e1.PropertyName=="IsActive")
                            {
                                if (!dt.IsActive && media.Source != null)
                                    media.Stop();

                            }
                        };
                    dt.OpenPDFViewer = (o2) =>
                        {
                            ShowPdfWindow(o2);
                        };
                };
            media.Play();
            media.MediaEnded += (o, e) =>
                {
                    if (media.Source == new Uri(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video1.mp4"))
                        media.Source = new Uri(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video2.mp4");
                    else
                        media.Source = new Uri(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video1.mp4");
                };
        }
        private void ShowPdfWindow(string path)
        {
            CustomWindow w = new CustomWindow();
            w.MinHeight = 610;
            w.MinWidth = 810;
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.WindowState = WindowState.Maximized;

            w.Content = new PDFViewVM(path);
            w.ShowDialog();
        }
    }
}
