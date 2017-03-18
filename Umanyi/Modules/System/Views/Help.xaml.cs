
using System.Security.Permissions;
using System.Windows;
using UmanyiSMS.Lib.Controls;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for AskForHelp.xaml
    /// </summary>
    
    public partial class Help :CustomWindow
    {
        
        public Help()
        {
            InitializeComponent();
        }
        private void OnWeblink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.almondtechnologies.kbo.co.ke");
            }
            catch { }
        }
        private void OnEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:rufw91@live.com");
            }
            catch { }
        }
        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
