using Helper;
using Helper.Controls;
using UmanyiSMS.ViewModels;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;
using Helper.Presentation;

namespace UmanyiSMS.Views
{

    public sealed partial class MainWindow : CustomWindow, IMainWindow
    {
        bool canClose = false;
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
            this.Closing += (o, e) =>
                {
                    if (canClose)
                        e.Cancel = false;
                    else
                    {
                        if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you would like to exit?\r\nAny unsaved data will be lost.", "Warning",
                           MessageBoxButton.YesNo, MessageBoxImage.Warning))
                            e.Cancel = false;
                        else e.Cancel = true;
                    }
                };
            
        }


        private void CustomWindow_Closed(object sender, EventArgs e)
        {
            if (!canClose)
            Application.Current.Shutdown();
        }

        internal void CloseWithoutPrompt()
        {
            canClose = true;
            Close();
        }
    }
}

    
