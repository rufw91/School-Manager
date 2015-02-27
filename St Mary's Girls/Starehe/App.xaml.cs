using Helper;
using Helper.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
namespace Starehe
{
    public partial class App : Application
    {
        public static ApplicationModel Info
        {
            get { return Helper.Properties.Settings.Default.Info; }
            private set { Helper.Properties.Settings.Default.Info = value; }

        }
        public static void Restart()
        {
            try
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            catch { }
        }
        private void InitGlobalVar()
        {
            bool isClient = false;
            if (Helper.Properties.Settings.Default.Info == null)
                Info = new ApplicationModel();
            else
            {
                Info = Helper.Properties.Settings.Default.Info;
            }
           // if (string.IsNullOrWhiteSpace(Helper.Properties.Settings.Default.ServerName))
                Helper.Properties.Settings.Default.ServerName = isClient?"BURSAR-PC\\Starehe":Environment.MachineName +
                    "\\Starehe";
            if (string.IsNullOrWhiteSpace(Helper.Properties.Settings.Default.DBName))
                Helper.Properties.Settings.Default.DBName = "Starehe";
            if (string.IsNullOrWhiteSpace(Helper.Properties.Settings.Default.MostRecentBackup))
                Helper.Properties.Settings.Default.MostRecentBackup = "";
            
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

            Helper.Properties.Settings.Default.PropertyChanged += (o, e) =>
                {
                    Helper.Properties.Settings.Default.Save();
                };
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitGlobalVar();
            FileHelper.CheckFiles();
            try
            {
                Login lg = new Login();
                lg.ShowDialog();
            }
            catch { }
            
        }
        
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {                       
            e.Handled = true;            
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {        
        }        
    }
}
