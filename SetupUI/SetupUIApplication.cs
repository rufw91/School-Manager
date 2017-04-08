using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace SetupUI
{
    public class SetupUIApplication : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; set; }
        public ActionResult ActionResult { get; private set; }
        IntPtr ptr = IntPtr.Zero;
        SetupUIViewModel viewModel = null;
        BootstrapperApplicationData bundleData;
        protected override void Run()
        {
            Thread.Sleep(new TimeSpan(0, 0, 2));
            bundleData = new BootstrapperApplicationData();
            Dispatcher = Dispatcher.CurrentDispatcher;
            viewModel = new SetupUIViewModel(this);
            var view = new SetupUIView();
            view.DataContext = viewModel;
            this.Engine.StringVariables["LogLocation"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Umanyi\UmanyiSMS\InstallLog.Log");
            ptr = new WindowInteropHelper(view).Handle;            
            this.Engine.Detect();
            this.Engine.CloseSplashScreen();
            view.Show();
            Dispatcher.Run();
            this.Engine.Quit(0);
        }

        private void GetAppData()
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dataXmlPath = Path.Combine(folder, "BootstrapperApplicationData.xml");
        }

        public void PlanAction(LaunchAction action)
        {
            this.Engine.Plan(action);
        }
        public void ApplyAction()
        {
            this.Engine.Apply(this.ptr);
        }

        

        private void OnApplyCompleted(object sender, ApplyCompleteEventArgs e)
        {
            this.ApplyComplete -= OnApplyCompleted;
            
            if (e.Status >= 0)
            {
                this.ActionResult = ActionResult.Success;
            }
            else
            {
                this.ActionResult = ActionResult.Failure;
            }

        }
        
        private void OnExecutedFilesInUse(object sender, ExecuteFilesInUseEventArgs e)
        {
            var message = new StringBuilder("The following files are in use. Please close the applications that are using them.n");
            foreach (var file in e.Files)
            {
                message.AppendLine(" - " + file);
            }

            var userButton = MessageBox.Show(message.ToString(), "Files In Use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (userButton != MessageBoxResult.OK)
                e.Result = Result.Cancel;

        }

        public BootstrapperApplicationData BundleData
        {
            get { return bundleData; }
        }

        public bool HasAdministratorRights
        {
            get
            {
                var pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

                return pricipal != null && pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        

    }
}
