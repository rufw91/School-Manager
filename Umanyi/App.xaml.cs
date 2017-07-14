
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
using log4net;
using log4net.Appender;
using System.IO;
using System.Text.RegularExpressions;

namespace UmanyiSMS
{
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "UmanyiSMS";
        private static ApplicationModel info;
        private IImmutableList<string> log;
        private static ExamSettingsModel examSettings;

        [STAThread]
        public static void Main(string[] args)
        {            
            XmlConfigurator.Configure();
            var appender = (FileAppender)LogManager.GetRepository().GetAppenders()[0];
            appender.File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Raphael Muindi\UmanyiSMS\Logs\LogFile.txt");
            appender.ActivateOptions();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            if (args != null && args.Length > 0 && args.Any(o => o.Trim().ToLowerInvariant() == "/b"))
                StartBackup();
            else if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
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

        private static void StartBackup()
        {
            if (string.IsNullOrWhiteSpace(Lib.Properties.Settings.Default.Info.DBName))
                Lib.Properties.Settings.Default.Info.DBName = "UmanyiSMS";
            Info = new ApplicationModel(Lib.Properties.Settings.Default.Info);
            string exp = @"^(?:""([^""]*)""\s*|([^""\s]+)\s*)+";
            var m = Regex.Match(Environment.CommandLine, exp);

            if (m.Groups.Count < 3)
                App.Current.Shutdown();

            var captures = m.Groups[1].Captures.Cast<Capture>().Concat(
                       m.Groups[2].Captures.Cast<Capture>()).
                       OrderBy(x => x.Index).
                       ToArray();

            if (captures.Length < 3)
                App.Current.Shutdown();

            if (captures[1].Value.ToLowerInvariant().Trim() == "/b")
            {
                string path = "";
                bool exists = false;
                try
                {
                    exists = File.Exists(captures[2].Value);
                }
                catch { exists = true; }
                if (exists)
                    path = FileHelper.GetDefaultBakPath();
                else
                    path = captures[2].Value;
                var t = SqlServerHelper.CreateInstance(info.ServerName,info.DBName,null,true).CreateBackupAsync(path);

                t.Wait();
            }
            App.Current.Shutdown();
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

                var m = (Application.Current.MainWindow as Window);
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
                

                examSettings = new ExamSettingsModel();
                
                
                if (string.IsNullOrWhiteSpace(Lib.Properties.Settings.Default.Info.DBName))
                    Lib.Properties.Settings.Default.Info.DBName = "UmanyiSMS";
                Info = new ApplicationModel(Lib.Properties.Settings.Default.Info);
                if (Info.Theme=="Dark")
                {
                    SetTheme("Dark");
                }
                SetAccent(info.AccentColor);
                Lib.Properties.Settings.Default.PropertyChanged += (o, e) =>
                {
                    Lib.Properties.Settings.Default.Save();
                };
                DataAccessHelper.Helper = SqlServerHelper.CreateInstance(info.ServerName,info.DBName,null, true);
            }
            catch (Exception e) { Log.E(e.ToString(), this); }
        }

        internal static void SetAccent(Color accentColor)
        {
            Application.Current.Resources["AccentColor"] = accentColor;
            Application.Current.Resources["Accent"] = new SolidColorBrush(accentColor);            
        }


        internal static void SetTheme(string theme)
        {
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


        internal static void SaveInfo(ApplicationModel appInfo)
        {
            Lib.Properties.Settings.Default.Info = new ApplicationPersistModel(appInfo);
            Lib.Properties.Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("/Resources/UmanyiSMSSplash.png");
            splashScreen.Show(true);

            info = new ApplicationModel();
            log = new ObservableImmutableList<string>();
            Log.Init(ref log);
            Log.I("Initializing...", this);

           
            if (Lib.Properties.Settings.Default.Info == null)
            {
                Lib.Properties.Settings.Default.Info = GetDefaultInfo();
                Lib.Properties.Settings.Default.Save();
            }

            InitGlobalVar();

            Window lg;
            if (RegistryHelper.IsFirstRun()) 
                lg = new SetupWizard(false);
            else lg = new Login();
           
            try
            {
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
            defaultInfo.Theme = "Light";
            defaultInfo.AccentColor = Color.FromRgb(100, 118, 135);
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
