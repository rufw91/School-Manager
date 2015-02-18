using Helper;
using Helper.Controls;
using Starehe.ViewModels;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;

namespace Starehe.Views
{

    public sealed partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();            
            var v = new MainWindowVM();
            v.AboutAction = () =>
                {
                    About a = new About();
                    a.ShowDialog();
                };
            DataContext = v;
        }


        private void CustomWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

    
