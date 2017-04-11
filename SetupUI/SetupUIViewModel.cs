using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SetupUI
{
    public class SetupUIViewModel : NotifiesPropertyChanged
    {
        public enum InstallState
        {
            Initializing,
            Present,
            NotPresent, Applying,
            Cancelled,
            Error,
            Complete
        }
        bool uninstall = false;
        private InstallState state;
        private int overallProgressPercentage;
        private string currentlyProcessingPackageName;
        private int currentComponentProgressPercentage;
        private string displayText;

        public SetupUIViewModel(SetupUIApplication app)
        {
            if (app != null)
            {
                app.DetectRelatedMsiPackage += (o, e) =>
                  {
                      if (true)
                      { }
                  };
                app.DetectRelatedBundle += (o, e) =>
                  {
                      if (true)
                      { }
                  };
                app.DetectComplete += (o, e) =>
                  {
                      SetupUIApplication.Dispatcher.Invoke((Action)delegate { CommandManager.InvalidateRequerySuggested(); });
                  };
                app.DetectPackageComplete += (o, e) =>
                  {
                      if (e.PackageId.Equals("UmanyiSMS", StringComparison.Ordinal))
                      {
                          this.State = e.State == PackageState.Present ? InstallState.Present : InstallState.NotPresent;
                      }
                      if (this.State == InstallState.Cancelled)
                      {
                          SetupUIApplication.Dispatcher.InvokeShutdown();
                          return;
                      }
                  };
                app.PlanComplete += (o, e) =>
                {
                    if (this.State == InstallState.Cancelled)
                    {
                        SetupUIApplication.Dispatcher.InvokeShutdown();
                        return;
                    }
                    app.ApplyAction();
                };
                app.Progress += (o, e) =>
                {
                    if (this.State == InstallState.Cancelled)
                    {
                        e.Result = Result.Cancel;
                    }
                    SetupUIApplication.Dispatcher.Invoke((Action)delegate
                    {
                        TaskbarManager.Instance.SetProgressValue(e.OverallPercentage, 100);
                    });
                    this.CurrentComponentProgressPercentage = e.ProgressPercentage;
                    this.OverallProgressPercentage = e.OverallPercentage;
                    DisplayText = CurrentlyProcessingPackageName + " " + e.OverallPercentage + " % ";
                };
                app.ApplyBegin += (o, e) =>
                  {
                      if (this.State == InstallState.Cancelled)
                      {
                          e.Result = Result.Cancel;
                      }
                      this.State = InstallState.Applying;
                  };
                app.ExecuteFilesInUse += (o, e) =>
                {
                    var message = new StringBuilder("The following files are in use. Please close the applications that are using them.");
                    foreach (var file in e.Files)
                    {
                        message.AppendLine(" - " + file);
                    }

                    var userButton = MessageBox.Show(message.ToString(), "Files In Use", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    
                        e.Result = Result.Cancel;
                };
                app.ExecutePackageBegin += (o, e) =>
                {

                    if (this.State == InstallState.Cancelled)
                    {
                        e.Result = Result.Cancel;
                    }
                    var inFlightPkgId = e.PackageId;
                    var inFlightPkg = app.BundleData.Data.Packages.FirstOrDefault(pkg => pkg.Id == inFlightPkgId);

                    if (inFlightPkg == null)
                    {
                        this.CurrentlyProcessingPackageName = e.PackageId;
                    }
                    else
                    {
                        this.CurrentlyProcessingPackageName = inFlightPkg.DisplayName;
                        this.DisplayText = CurrentlyProcessingPackageName;
                    }
                };
                app.ExecutePackageComplete += (o, e) =>
                {
                    if (e.Status < 0)
                    {
                        State = InstallState.Error;
                    }
                    if (this.State == InstallState.Cancelled)
                    {
                        e.Result = Result.Cancel;
                    }
                };
                app.ApplyComplete += (o, e) =>
                {
                    OverallProgressPercentage = 100;
                    DisplayText = string.Format("Operation Finished. 100%");
                    if (e.Status < 0)
                    {
                        SetupUIApplication.Dispatcher.Invoke((Action)delegate
                        {
                            TaskbarManager.Instance.SetProgressValue(100, 100);
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                        });
                        State = InstallState.Error;
                        SetupUIApplication.Dispatcher.Invoke(OpenPage8Action);
                    }
                    else
                    {
                        State = InstallState.Complete;
                        SetupUIApplication.Dispatcher.Invoke(uninstall ? OpenPage9Action : OpenPage3Action);
                    }
                };

            }
            this.InstallCommand = new RelayCommand(p => {
                State = InstallState.Applying;
                if (OpenPage2Action != null)
                    OpenPage2Action.Invoke();
                app.PlanAction(app.Command.Action);
            }, p => app.Command.Action == LaunchAction.Install);
            this.UninstallCommand = new RelayCommand(p => {
                State = InstallState.Applying;
                uninstall = true;
                if (OpenPage2Action != null)
                    OpenPage2Action.Invoke();
                app.PlanAction(app.Command.Action);
            },
            o => app.Command.Action == LaunchAction.Uninstall);

            this.CancelCommand = new RelayCommand(p => {
                if (OpenPage6Action != null)
                    OpenPage6Action.Invoke();
            },
            o => true);
            this.YesCommand = new RelayCommand(p => {
                if (this.State == (InstallState.Applying | InstallState.Initializing))
                {
                    if (OpenPage5Action != null)
                        OpenPage5Action.Invoke();
                    this.State = InstallState.Cancelled;
                }
                else
                    SetupUIApplication.Dispatcher.InvokeShutdown();
            },
            o => true);
            this.NoCommand = new RelayCommand(p => {
                if (OpenPage2Action != null)
                    OpenPage2Action.Invoke();
            },
            o => true);
            this.FinishCommand = new RelayCommand(p => {
                {
                    if (LaunchApp)
                    {
                        RunAsAdmin(GetExecIntallPath(), "");
                    }
                    SetupUIApplication.Dispatcher.InvokeShutdown();
                }
            },
            o => true);
            this.InstallationLogCommand = new RelayCommand(p => {
                try
                {
                    Process.Start(app.Engine.StringVariables["LogLocation"]);
                }
                catch (Exception) { }
            },
            o => true);
        }
        private string GetExecIntallPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Raphael Muindi\Umanyi School MS\UmanyiSMS.exe");
        }
        private void RunAsAdmin(string fileName, string args)
        {
            var processInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = fileName,
                Arguments = args,
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Win32Exception)
            {
                // Do nothing...
            }
        }

        public int CurrentComponentProgressPercentage
        {
            get { return currentComponentProgressPercentage; }

            set
            {
                if (value != this.currentComponentProgressPercentage)
                {
                    this.currentComponentProgressPercentage = value;
                    NotifyPropertyChanged("CurrentComponentProgressPercentage");
                }
            }
        }
        public string CurrentlyProcessingPackageName
        {
            get { return currentlyProcessingPackageName; }

            set
            {
                if (value != this.currentlyProcessingPackageName)
                {
                    this.currentlyProcessingPackageName = value;
                    NotifyPropertyChanged("CurrentlyProcessingPackageName");
                }
            }
        }

        public string DisplayText
        {
            get { return displayText; }

            set
            {
                if (value != this.displayText)
                {
                    this.displayText = value;
                    NotifyPropertyChanged("DisplayText");
                }
            }
        }
        public bool LaunchApp
        {
            get;
            set;
        }
        public Action OpenPage2Action
        {
            get;
            set;
        }
        public Action OpenPage3Action
        {
            get;
            set;
        }
        public Action OpenPage4Action
        {
            get;
            set;
        }
        public Action OpenPage5Action
        {
            get;
            set;
        }
        public Action OpenPage6Action
        {
            get;
            set;
        }
        public Action OpenPage7Action
        {
            get;
            set;
        }
        public Action OpenPage8Action
        {
            get;
            set;
        }
        public Action OpenPage9Action
        {
            get;
            set;
        }
        public ICommand InstallationLogCommand
        { get; private set; }

        public ICommand InstallCommand
        {
            get;
            private set;
        }
        public ICommand UninstallCommand
        {
            get;
            private set;
        }
        public ICommand CancelCommand
        {
            get;
            private set;
        }
        public ICommand YesCommand
        {
            get;
            private set;
        }
        public ICommand NoCommand
        {
            get;
            private set;
        }
        public ICommand FinishCommand
        {
            get;
            private set;
        }
        
        public int OverallProgressPercentage
        {
            get { return overallProgressPercentage; }

            set
            {
                if (value != this.overallProgressPercentage)
                {
                    this.overallProgressPercentage = value;
                    NotifyPropertyChanged("OverallProgressPercentage");
                }
            }
        }
        
        public InstallState State
        {
            get { return state; }

            set
            {
                if (value != this.state)
                {
                    this.state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }
    }
}
