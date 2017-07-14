using log4net;
using log4net.Appender;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Properties;

namespace DataTransfer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string LogFilePath { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Raphael Muindi\UmanyiSMS\Logs\DataTransferLogFile.txt");
            XmlConfigurator.Configure();
            var appender = (FileAppender)LogManager.GetRepository().GetAppenders()[0];
            appender.File = LogFilePath;
            appender.ActivateOptions();
            IImmutableList<string> y = ImmutableList.Create<string>();
            Log.Init(ref y);
            MainWindow mn = new DataTransfer.MainWindow();
            mn.ShowDialog();
        }
        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
