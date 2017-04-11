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
