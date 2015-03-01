using Helper.Controls;
using System.Windows;

namespace Starehe.Views.FirstRun
{
    public partial class FirstRun : CustomWindow
    {
        bool canClose = true;
        public FirstRun()
        {
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
