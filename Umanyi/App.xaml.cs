using Helper;
using Helper.Controls;
using Helper.Helpers;
using Helper.Models;
using Helper.Presentation;
using log4net.Config;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UmanyiSMS.ViewModels;
using UmanyiSMS.Views;

namespace UmanyiSMS
{
    public partial class App : Application, ISingleInstanceApp, IApp
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
                FileHelper.CheckFiles();
                examSettings = new ExamSettingsModel();
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
            catch (Exception e) { Log.E(e.ToString(), this); }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen("/Resources/Starehe0078C8.png");
            splashScreen.Show(true);

            info = new ApplicationModel();
            log = new ObservableImmutableList<string>();
            Log.Init(ref log);
            Log.I("Init Vars", this);

            Helper.Properties.Settings.Default.Info = GetDefaultInfo();
            Helper.Properties.Settings.Default.Save();


            if (Helper.Properties.Settings.Default.Info == null)
            {
                GenericIdentity MyIdentity = new GenericIdentity("DefaultSetupUser");
                GenericPrincipal MyPrincipal =
                    new GenericPrincipal(MyIdentity, new string[] { "Deputy" });
                AppDomain.CurrentDomain.SetThreadPrincipal(MyPrincipal);
                CustomWindow cm = new CustomWindow();
                cm.Content = new InstitutionSetupVM(true);
                cm.MinHeight = 610;
                cm.MinWidth = 810;
                cm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cm.WindowState = WindowState.Maximized;
                bool canClose = false;
                bool isExiting = false;
                cm.DataContextChanged += (o, e1) =>
                {
                    InstitutionSetupVM vm = cm.DataContext as InstitutionSetupVM;
                    if (vm != null)
                        vm.CloseWindowAction = () =>
                        {
                            canClose = true;
                            cm.Close();
                        };

                };
                cm.Closed += (o2, e2) =>
                {
                    if (!canClose)
                        isExiting = true;
                };
                cm.ShowDialog();
                if (isExiting)
                {
                    Shutdown();
                    return;
                }

                Restart();
                return;
            }

            InitGlobalVar();


            /* if (!await ActivationHelper.LicenseExists())
                 new InvalidLicense().ShowDialog();
             */
            // if (await ActivationHelper.IsActivated())
            //{
            try
            {
                Login lg = new Login();
                MainWindow = lg;
                lg.ShowDialog();
            }
            catch { }
            /*}
            else
            {
                try
                {
                    Activation a = new Activation();
                    MainWindow = a;
                    a.ShowDialog();
                }
                catch { }
            }*/

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
