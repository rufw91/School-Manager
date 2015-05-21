using Helper;
using Helper.Helpers;
using Helper.Models;
using log4net;
using log4net.Config;
using Microsoft.Shell;
using UmanyiSMS.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Interop;
namespace UmanyiSMS
{
    public partial class App : Application, ISingleInstanceApp 
    {       
        private const string Unique = "UmanyiSMS";
        private static ApplicationModel info;
        [STAThread]
        public static void Main()
        {
            XmlConfigurator.Configure();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
               
                var application = new App();

                application.InitializeComponent();
                application.Run();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }
        #region ISingleInstanceApp Members
        
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow == null)
                return true;
            if (this.MainWindow.WindowState == WindowState.Minimized)
                this.MainWindow.WindowState = WindowState.Normal;
            this.MainWindow.Activate();

            return true;
        }

        #endregion
        public static ApplicationModel Info
        {
            get { return info; }
            private set
            {
                if (info != value)
                    info = value;
            }
        }
        public static void Restart()
        {
            try
            {
                
                MainWindow m = (Application.Current.MainWindow as MainWindow);
                if (m != null)
                {
                    m.Visibility = Visibility.Collapsed;
                    m.CloseWithoutPrompt();
                }
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            catch { }
        }

        private void InitGlobalVar()
        {
            try
            {
                if (Helper.Properties.Settings.Default.Info == null)
                    Helper.Properties.Settings.Default.Info = new ApplicationPersistModel(new ApplicationModel());
                Info = new ApplicationModel(Helper.Properties.Settings.Default.Info);
                if (string.IsNullOrWhiteSpace(Helper.Properties.Settings.Default.DBName))
                    Helper.Properties.Settings.Default.DBName = "UmanyiSMS";
                if (string.IsNullOrWhiteSpace(Helper.Properties.Settings.Default.MostRecentBackup))
                    Helper.Properties.Settings.Default.MostRecentBackup = "";

                Helper.Properties.Settings.Default.PropertyChanged += (o, e) =>
                    {
                        Helper.Properties.Settings.Default.Save();
                    };
            }
            catch(Exception e) { Log.E(e.ToString(),this); }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("/Resources/Starehe0078C8.png");
            splashScreen.Show(true);

            Log.I("Init Vars",this);
            InitGlobalVar();
            FileHelper.CheckFiles();
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            if (ActivationHelper.IsActivated().Result)
            {
                try
                {

                    Login lg = new Login();
                    MainWindow = lg;
                    lg.ShowDialog();
                }
                catch { }
            }
            else
            {
                try
                {
                    Activation a = new Activation();
                    MainWindow = a;
                    a.ShowDialog();
                }
                catch { }
            }

        }
        
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.E(e.Exception.ToString(), this);
            e.Handled = true;            
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {        
        }
    }
}
