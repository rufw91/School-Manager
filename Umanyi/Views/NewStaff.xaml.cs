using UmanyiSMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class NewStaff : UserControl
    {
        public NewStaff()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    NewStaffVM nsvm = (NewStaffVM)DataContext;
                    nsvm.ResetAction = () =>
                    {
                        txtPwd.Password = "";
                        txtPwd2.Password = "";
                    };
                }
            };
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if ((txtPwd.Password == txtPwd2.Password) && (txtPwd.Password.Length >= 1))
            {
                (this.DataContext as NewStaffVM).SecurePassword = txtPwd.SecurePassword.Copy();
            }
            else (this.DataContext as NewStaffVM).SecurePassword = null;
        }
    }
}
