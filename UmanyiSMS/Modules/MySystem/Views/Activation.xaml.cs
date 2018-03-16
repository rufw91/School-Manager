

using System.Windows;
using UmanyiSMS.Lib.Controls;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class Activation : CustomWindow
    {
        private bool canClose = false;
        public Activation():this(false)
        {
           
        }

        public Activation(bool isA)
        {
            InitializeComponent();
            if (!isA)
            {
                this.Closed += (o, e) =>
                {
                    if (!canClose)
                        App.Current.Shutdown(8900);
                };
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool succ = false;// await ActivationHelper.Activate(txtLicense.Text);
            MessageBox.Show(succ ? "Successfully activated application." : "Could not activate. Ensure you have a working internet connection and try again.",
                succ ? "Success" : "Error", MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
            if (succ)
            {
                canClose = true;
                Login main = new Login();
                Application.Current.MainWindow = main;
                main.Show();
                this.Close();
            }
        }
    }
}
