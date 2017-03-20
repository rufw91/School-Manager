
using System.Windows;
using UmanyiSMS.Lib.Controls;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class AppFirstRun : CustomWindow
    {
        bool canClose;
        public AppFirstRun()
        {
            canClose = true;
            InitializeComponent();
            Closing += (o, e) =>
                {
                    if (canClose)
                    {
                        MainWindow main = new MainWindow();
                        Application.Current.MainWindow = main;
                        main.Show();
                        canClose = false;
                    }
                    else
                        e.Cancel = true;
                };
        }
    }
}
