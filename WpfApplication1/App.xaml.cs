using SetupUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetupUI.SetupUIView v = new SetupUI.SetupUIView();
            //v.DataContext = new SetupUIViewModel(null);
            // v.ShowDialog();

            
            Window t = new Window();
            t.Show();
            TaskbarManager.Instance.SetProgressValue(78, 100);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            
            

        }
    }
}
