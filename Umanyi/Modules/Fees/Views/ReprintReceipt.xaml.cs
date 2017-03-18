

using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Fees.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class ReprintReceipt : UserControl
    {
        public ReprintReceipt()
        {
            InitializeComponent();
            viewer.FitToWidth();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    ReprintReceiptVM nfpvm = DataContext as ReprintReceiptVM;
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
