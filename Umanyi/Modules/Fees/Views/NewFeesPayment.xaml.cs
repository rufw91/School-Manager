

using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Fees.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Fees.Views
{
    public partial class NewFeesPayment : UserControl
    {
        public NewFeesPayment()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
                {
                    if (DataContext != null)
                    {
                        NewFeesPaymentVM nfpvm = DataContext as NewFeesPaymentVM;
                        nfpvm.ShowPrintDialogAction = (p) =>
                            {
                                CustomWindow w = new CustomWindow();
                                w.MinHeight = 610;
                                w.MinWidth = 810;
                                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                w.WindowState = WindowState.Maximized;

                                w.Content = new MyPrintDialog(p);
                                w.ShowDialog();

                            };
                    }
                };
        }
    }
}
