


using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;

using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{

    public sealed partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();
                    
            var v = new MainWindowVM();
                v.HelpGetHelpAction = () =>
                {
                    MessageBox.Show("Contact your system administrator for assistance.","Help", MessageBoxButton.OK, MessageBoxImage.Information);
                };

            v.HelpAboutAction = () =>
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

    
