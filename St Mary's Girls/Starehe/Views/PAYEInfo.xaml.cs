using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
{
    public partial class PAYEInfo : UserControl
    {
        public PAYEInfo()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
           {
               if (DataContext != null)
               {
                   PAYEInfoVM nfpvm = DataContext as PAYEInfoVM;
                   nfpvm.OpenKRAGuideAction = (p) =>
                   {
                       ShowPdfWindow(p);
                   };
               }
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
