
using System.Windows;
using UmanyiSMS.Lib.Controls;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class InvalidLicense : CustomWindow
    {
        public InvalidLicense()
        {
            InitializeComponent();
            this.Closed += (o, e) =>
            {
                    App.Current.Shutdown(8901);
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
