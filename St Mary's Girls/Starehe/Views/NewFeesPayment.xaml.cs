using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
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

                                w.Content = new PrintDialog(p);
                                w.ShowDialog();

                            };
                    }
                };
        }
    }
}
