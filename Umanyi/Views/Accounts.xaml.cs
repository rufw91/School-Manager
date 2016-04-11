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
    public partial class Accounts : UserControl
    {
        public Accounts()
        {
            Window w = null;
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                AccountsVM dbtvm = this.DataContext as AccountsVM;
                if (dbtvm == null)
                    return;
                dbtvm.OpenTaskWindowAction = (p) =>
                {
                    w = new CustomWindow();

                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;


                    w.Content = p;
                    w.ShowDialog();
                };
            };
        }

    }
}
