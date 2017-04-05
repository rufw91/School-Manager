
using log4net.Config;
using Microsoft.Shell;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.MySystem.Views;
using System.Windows.Media;

namespace UmanyiSMS
{
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "UmanyiSMS";
        private static ApplicationModel info;
        private IImmutableList<string> log;
        private static ExamSettingsModel examSettings;

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

        public static ExamSettingsModel AppExamSettings
        {
            get { return examSettings; }
        }

        public IImmutableList<string> LogEntries
        {
            get { return log; }
        }

        public ApplicationModel AppInfo
        {
            get { return App.info; }
        }

        public ExamSettingsModel ExamSettings
        {
            get { return App.AppExamSettings; }
        }

        public static void Restart()
        {
            try
            {

                MainWindow m = (Application.Current.MainWindow as MainWindow);
                if (m != null)
                {
                    m.Visibility = Visibility.Collapsed;
                    m.Close();
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
                DataAccessHelper.Helper = new SqlServerHelper(null);
                FileHelper.CheckFiles();
                examSettings = new ExamSettingsModel();
                
                Info = new ApplicationModel(Lib.Properties.Settings.Default.Info);
                if (string.IsNullOrWhiteSpace(Lib.Properties.Settings.Default.Info.DBName))
                    Lib.Properties.Settings.Default.Info.DBName = "UmanyiSMS";
                if (Info.Theme=="Dark")
                {
                    SetTheme("Dark");
                }
                Lib.Properties.Settings.Default.PropertyChanged += (o, e) =>
                {
                    Lib.Properties.Settings.Default.Save();
                };

            }
            catch (Exception e) { Log.E(e.ToString(), this); }
        }

        internal static void SetAccent(Color accentColor)
        {
            Application.Current.Resources["AccentColor"] = accentColor;
            Application.Current.Resources["Accent"] = new SolidColorBrush(accentColor);
            App.Info.AccentColor = accentColor;
            App.SaveInfo();
        }


        internal static void SetTheme(string theme)
        {
            App.Info.Theme = theme;
            App.SaveInfo();
            Uri source;
            if (theme == "Light")
                source = new Uri("pack://application:,,,/UmanyiSMS.Lib;component/Themes/Theme.Light.xaml");
            else
                source = new Uri("pack://application:,,,/UmanyiSMS.Lib;component/Themes/Theme.Dark.xaml");
            ResourceDictionary themeDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(ox => ox.Contains("WindowBackground"));
            System.Collections.ObjectModel.Collection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = new ResourceDictionary
            {
                Source = source
            };

            mergedDictionaries.Add(resourceDictionary);
            if (themeDictionary != null)
            {
                mergedDictionaries.Remove(themeDictionary);
            }
        }


        internal static void SaveInfo()
        {
            Lib.Properties.Settings.Default.Info = new ApplicationPersistModel(Info);
            Lib.Properties.Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("/Resources/UmanyiSMSSplash.png");
            splashScreen.Show(true);

            info = new ApplicationModel();
            log = new ObservableImmutableList<string>();
            Log.Init(ref log);
            Log.I("Init Vars", this);

           
            if (Lib.Properties.Settings.Default.Info == null)
            {
                Lib.Properties.Settings.Default.Info = GetDefaultInfo();
                Lib.Properties.Settings.Default.Save();
            }

            InitGlobalVar();
            
            try
            {
                Login lg = new Login();
                MainWindow = lg;
                lg.ShowDialog();
            }
            catch { }
            
        }

        private ApplicationPersistModel GetDefaultInfo()
        {
            ApplicationModel defaultInfo = new ApplicationModel();
            defaultInfo.Address = "80 - 90108";
            defaultInfo.AltInfo = "Catholic Mission";
            defaultInfo.City = "Kola";
            defaultInfo.Email = "stmarysgirlskola@yahoo.com";
            defaultInfo.FullName = "ST MARYS GIRLS - KOLA";
            defaultInfo.FullNameAlt = "St Marys Girls Secondary School";
            defaultInfo.Motto = "In Pursuit of Excellence";
            defaultInfo.Name = "St Marys Girls";
            defaultInfo.PhoneNo = "+254 721 437 475";
            defaultInfo.SPhoto = null;

            return new ApplicationPersistModel(defaultInfo);
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
