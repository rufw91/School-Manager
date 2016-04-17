using Helper.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for AccountsGeneralLedger.xaml
    /// </summary>
    public partial class AccountsGeneralLedger : UserControl
    {
        public AccountsGeneralLedger()
        {
            InitializeComponent();
            viewer.FitToWidth();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    AccountsGeneralLedgerVM nfpvm = DataContext as AccountsGeneralLedgerVM;
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
