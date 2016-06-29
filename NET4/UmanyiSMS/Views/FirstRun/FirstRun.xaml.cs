using Helper.Controls;
using System.Windows;

namespace UmanyiSMS.Views.FirstRun
{
    public partial class FirstRun : CustomWindow
    {
        bool canClose;
        public FirstRun()
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
