

using System;
using System.Data.SqlClient;
using System.Security;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Views;

using UmanyiSMS.Views.FirstRun;
using System.Threading;
using System.Security.Principal;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.System.ViewModels;
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS
{
    public partial class Login : CustomWindow
    {
        LoginType _type;
        Action _actionToExecute;
        bool shutDownApp { get; set; }
        public Login()
        {
            _type = LoginType.Login;
            InitializeComponent();
            shutDownApp = true;
            this.Closed += (o, e) =>
            {
                if (shutDownApp)
                    Application.Current.Shutdown();
            };
            txtPwd.PasswordChanged += (o, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtPwd.Password))
                        txtPwd.Tag = "Password";
                    else
                        txtPwd.Tag = "";
                };
            FocusManager.SetFocusedElement(this, txtUId);
        }
        public Login(LoginType type)
        {
            _type = type;
            InitializeComponent();
            if (type == LoginType.Login)
            {
                shutDownApp = true;
                this.Closed += (o, e) =>
                {
                    if (shutDownApp)
                        Application.Current.Shutdown();
                };
            }
            txtPwd.PasswordChanged += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPwd.Password))
                    txtPwd.Tag = "Password";
                else
                    txtPwd.Tag = "";
            };
        }
        public Login(LoginType type, ref Action actionToExecute)
        {
            _type = type;
            _actionToExecute = actionToExecute;
            InitializeComponent();
            if (type == LoginType.Login)
            {
                shutDownApp = true;
                this.Closed += (o, e) =>
                {
                    if (shutDownApp)
                        Application.Current.Shutdown();
                };
            }
            txtPwd.PasswordChanged += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPwd.Password))
                    txtPwd.Tag = "Password";
                else
                    txtPwd.Tag = "";
            };
        }

        
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            mnGrid.IsEnabled = false;
            SecureString p = txtPwd.SecurePassword.Copy();
            p.MakeReadOnly();
            SqlCredential c = new SqlCredential(txtUId.Text, p);
            bool authentic = await LoginHelper.AuthenticateUser(c);

            if (txtUId.Text.ToLowerInvariant()=="sa")
            {
                MessageBox.Show("The System Account 'sa' will be disabled in future versions of Umanyi MS, consider creating new user accounts  (by adding staff).", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            mnGrid.IsEnabled = true;
            if (!authentic)
                MessageBox.Show("Invalid User ID or Password. \r\nFor help, contact your system admin.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                App.Info.CurrentUser = UsersHelper.CurrentUser;
                if (_type == LoginType.Login)
                {
                   
                        MainWindow main = new MainWindow();
                        Application.Current.MainWindow = main;
                        main.Show();
                        shutDownApp = false;
                        this.Close();
                }
                else
                {
                    this.Visibility = System.Windows.Visibility.Collapsed;
                    if (_actionToExecute != null)
                    { _actionToExecute.Invoke(); }
                    this.Close();
                }
            }
        }
        
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            
            CustomWindow w = new CustomWindow();
            w.ResizeMode= System.Windows.ResizeMode.NoResize;
            w.WindowStartupLocation= System.Windows.WindowStartupLocation.CenterScreen;
            w.MaxHeight = 400;
            w.MaxWidth = 600;
            w.MinHeight = 400;
            w.MinWidth = 600;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("temp_sa"), new string[1] { "SystemAdmin" });
            var i = new NetworkOptionsVM(false);
            w.Content = i;
            w.ShowDialog();
            i = null;
            w = null;
            Thread.CurrentPrincipal = null;
        }
    }
}
