
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Purchases.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class ReprintPaymentVoucher : UserControl
    {
        public ReprintPaymentVoucher()
        {
            InitializeComponent();
            viewer.FitToWidth();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    ReprintPaymentVoucherVM nfpvm = DataContext as ReprintPaymentVoucherVM;
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
