using Helper;
using Helper.Controls;
using System;
using System.Data.SqlClient;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Starehe.Views;

namespace Starehe
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
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sc = new SqlConnection("Data Source=Bursar-PC\\Starehe; Integrated Security=false; Persist Security Info=True; User ID=sa; Password=000002");
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            mnGrid.IsEnabled = false;
            SecureString p = txtPwd.SecurePassword.Copy();
            p.MakeReadOnly();
            SqlCredential c = new SqlCredential(txtUId.Text, p);
            bool authentic = await LoginHelper.AuthenticateUser(c);
            mnGrid.IsEnabled = true;
            if (!authentic)
                MessageBox.Show("Invalid User ID or Password. \r\nFor help, contact your system admin.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                if (_type == LoginType.Login)
                {
                    this.Visibility = System.Windows.Visibility.Collapsed;
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
    }
}
