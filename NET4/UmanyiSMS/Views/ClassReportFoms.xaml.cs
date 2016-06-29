using Helper.Controls;
using UmanyiSMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class ClassReportForms : UserControl
    {
        public ClassReportForms()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    ClassReportFormsVM nfpvm = DataContext as ClassReportFormsVM;
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
