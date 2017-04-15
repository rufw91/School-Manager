
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class ChangeSAPassword : UserControl
    {
        public ChangeSAPassword()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeSAPasswordVM dc = DataContext as ChangeSAPasswordVM;
            dc.IsBusy = true;
            if (oPwd.SecurePassword == null || nPwd1.SecurePassword==null||nPwd2.SecurePassword==null||
                oPwd.SecurePassword.Length < 5 || nPwd1.SecurePassword.Length < 5 || nPwd2.SecurePassword.Length < 5)
            {
                MessageBox.Show("Password too short! Must be atleast 5 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                dc.IsBusy = false;
                return;
            }
            SecureString o1 = oPwd.SecurePassword;
            SecureString n1 = nPwd1.SecurePassword;
            SecureString n2 = nPwd2.SecurePassword;

            o1.MakeReadOnly();
            n1.MakeReadOnly();
            n2.MakeReadOnly();

            if (!TestOldPassword(new SqlCredential("sa", o1)))
            {
                MessageBox.Show("Invalid Password. Please enter the current password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                dc.IsBusy = false;
                return;
            }
            if (nPwd1.Password!=nPwd2.Password)
            {
                MessageBox.Show("The two passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                dc.IsBusy = false;
                return;
            }
            
           bool succ= await UsersHelper.UpdateUserAsync(new SqlCredential("sa", n1), UserRole.SystemAdmin);
           MessageBox.Show(succ ? "Successfully completed operation." : "Could not complete opretaion.", "Information", MessageBoxButton.OK,
               succ ? MessageBoxImage.Information : MessageBoxImage.Warning
               );
           oPwd.Password = "";
           nPwd1.Password = "";
           nPwd2.Password = "";
           dc.IsBusy = false;
        }

        internal bool TestOldPassword(SqlCredential cred)
        {
            SqlConnection conn;
            try
            {
                conn = new SqlConnection(ConnectionStringHelper.GetConnectionString());
                conn.Credential = cred;
                conn.Open();
                if (conn.State == ConnectionState.Connecting)
                    while (conn.State != ConnectionState.Open)
                    { }
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(DataAccessHelper));
            }
            return false;
        }
    }
}
