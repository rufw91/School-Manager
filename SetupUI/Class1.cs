using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace SetupUI
{
    public class SetupUIApplication : BootstrapperApplication
    {
        public static Dispatcher Dispatcher { get; set; }
        public ActionResult ActionResult { get; private set; }

        SetupUIViewModel viewModel = null;
        private bool _userHasCancelled;

        protected override void Run()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            var model = new SetupUIModel();
            viewModel = new SetupUIViewModel();
            var view = new SetupUIView();
            view.DataContext = viewModel;
            this.Engine.Apply(new WindowInteropHelper(view).Handle);
            view.Closed += (sender, e) => Dispatcher.InvokeShutdown();

            this.ExecuteMsiMessage += OnExecutedMsiMessage;
            this.Progress += OnProgressed;
            this.ExecuteComplete += OnExecuteCompleted;
            this.ExecuteFilesInUse += OnExecutedFilesInUse;
            this.ExecutePackageBegin += OnExecutedPackageBegin;
            this.ExecutePackageComplete += OnExecutedPackageComplete;
            this.ApplyComplete += OnApplyCompleted;


            /*this.DetectRelatedBundle += HandleExistingBundleDetected;

            // This is called when a package in the bundle is detected
            this.DetectPackageComplete += SetPackageDetectedState;

            // This is called when a package in the bundle is detected
            this.DetectRelatedMsiPackage += HandleExistingPackageDetected;

            // This is called when a Feature in the bundle's packages is detected
            this.DetectMsiFeature += SetFeatureDetectedState;
            DetectComplete += DetectCompleted;
            */
            this.Engine.Detect();
            view.Show();
            Dispatcher.Run();
            this.Engine.Quit(0);
        }

        private void DetectCompleted(object sender, DetectCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SetFeatureDetectedState(object sender, DetectMsiFeatureEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleExistingPackageDetected(object sender, DetectRelatedMsiPackageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SetPackageDetectedState(object sender, DetectPackageCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleExistingBundleDetected(object sender, DetectRelatedBundleEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnApplyCompleted(object sender, ApplyCompleteEventArgs e)
        {
            this.ApplyComplete -= OnApplyCompleted;

            //using "ActionResult" property to store the result for use
            // when I call Engine.Quit()

            if (e.Status >= 0)
            {
                this.ActionResult = ActionResult.Success;
            }
            else
            {
                this.ActionResult = ActionResult.Failure;
            }

        }

        private void OnExecutedPackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnExecutedPackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            /*var inFlightPkgId = e.PackageId;
            var inFlightPkg = BundlePackages.FirstOrDefault(pkg => pkg.Id == inFlightPkgId);

            if (inFlightPkg == null)
            {
                viewModel.CurrentlyProcessingPackageName = string.Empty;
            }
            else
            {
                viewModel.CurrentlyProcessingPackageName = inFlightPkg.Name;
            }*/

        }

        private void OnExecuteCompleted(object sender, ExecuteCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnProgressed(object sender, ProgressEventArgs e)
        {
            if (_userHasCancelled)
                e.Result = Result.Cancel;

            viewModel.CurrentComponentProgressPercentage = e.ProgressPercentage;
            viewModel.OverallProgressPercentage = e.OverallPercentage;

        }

        private void OnExecutedMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            throw new NotImplementedException();
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
