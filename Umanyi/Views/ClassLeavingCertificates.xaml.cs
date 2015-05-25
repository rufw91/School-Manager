using Helper.Controls;
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class ClassLeavingCertificates : UserControl
    {
        public ClassLeavingCertificates()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    ClassLeavingCertificatesVM nfpvm = DataContext as ClassLeavingCertificatesVM;
                    nfpvm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new PrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
