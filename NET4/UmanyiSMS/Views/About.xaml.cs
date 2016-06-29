using Helper.Controls;
using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows;
using Helper;
using System.Diagnostics;
using System.Reflection;

namespace UmanyiSMS
{
    
    public partial class About :CustomWindow
    {
        public About()
        {
            InitializeComponent();
            this.DataContext = Credits.TheCredits;

            txtVersion.Text = "Version "+typeof(App).Assembly.GetName().Version.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnWeblink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://www.umanyi.co.ke/solutions");
            }
            catch { }
        }
        private void OnLicenceLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.gnu.org/licenses/lgpl.html");
            }
            catch { }
        }

    }
}
