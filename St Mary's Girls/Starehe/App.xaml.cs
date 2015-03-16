﻿using Helper;
using Helper.Helpers;
using Helper.Models;
using log4net;
using log4net.Config;
using Microsoft.Shell;
using Starehe.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
namespace Starehe
{
    public partial class App : Application, ISingleInstanceApp 
    {       
        private const string Unique = "Starehe";
        private static ApplicationModel info;
        [STAThread]
        public static void Main()
        {
            XmlConfigurator.Configure();
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }
        #region ISingleInstanceApp Members
        
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // …

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
                m.Visibility = Visibility.Collapsed;
                m.CloseWithoutPrompt();
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
            catch(Exception e) { Log.E(e.ToString(),this); }
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Log.I("Init Vars",this);
            InitGlobalVar();
            FileHelper.CheckFiles();
            if (await ActivationHelper.IsActivated())
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
