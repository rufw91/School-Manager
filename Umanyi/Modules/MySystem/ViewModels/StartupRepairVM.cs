
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.MySystem.Models;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class StartupRepairVM: ViewModelBase
    {
        StartUpModel start;
        TaskStates creatingBackup;
        TaskStates repairingInstallation;
        TaskStates repairingInstance;
        TaskStates repairingDefaultLogin;
        TaskStates repairingDb;
        TaskStates repairingDbObjects;
        bool repairInProgress;
        bool showDetails;
        bool repairComplete;
        bool showErrors;
        string message;

        public StartupRepairVM(StartUpModel args)
        {
            start = args;
            InitVars();
            CreateCommands();                 
        }

        protected override void InitVars()
        {            
            creatingBackup = TaskStates.Idle;
            repairingInstallation = TaskStates.Idle;
            repairingInstance = TaskStates.Idle;
            repairingDefaultLogin = TaskStates.Idle;
            repairingDb = TaskStates.Idle;
            repairingDbObjects = TaskStates.Idle;
            RepairInProgress = false;
            RepairComplete = false;
            ShowDetails = false;
            ShowErrors = true;
            Message = "We have detected a few problems and are trying to fix this. Please be patient this will only take a few minutes.";
        }

        protected override void CreateCommands()
        {
            
            ShowDetailsCommand = new RelayCommand(o => { ShowDetails = !ShowDetails; }, o => { return true; });
            CloseCommand = new RelayCommand(o => { CloseAction.Invoke(); }, o => { return true; });
            StartRepairCommand = new RelayCommand(o => { StartRepair(start); }, o => true);       
        }
        
        public ObservableCollection<Error> Errors
        { get { return start.Errors; } }

        public Action CloseAction
        { get; set; }

        public ICommand CloseCommand
        {
            get;
            private set;
        }

        public ICommand ShowDetailsCommand
        {
            get;
            private set;
        }

        public ICommand StartRepairCommand
        {
            get;
            private set;
        }

        public TaskStates CreatingBackup
        {
            get { return creatingBackup; }
            set
            {
                if (value != this.creatingBackup)
                {
                    this.creatingBackup = value;
                    NotifyPropertyChanged("CreatingBackup");
                }
            }
        }

        public TaskStates RepairingInstallation
        {
            get { return repairingInstallation; }
            set
            {
                if (value != this.repairingInstallation)
                {
                    this.repairingInstallation = value;
                    NotifyPropertyChanged("RepairingInstallation");
                }
            }
        }

        public TaskStates RepairingInstance
        {
            get { return repairingInstance; }
            set
            {
                if (value != this.repairingInstance)
                {
                    this.repairingInstance = value;
                    NotifyPropertyChanged("RepairingInstance");
                }
            }
        }

        public TaskStates RepairingDefaultLogin
        {
            get { return repairingDefaultLogin; }
            set
            {
                if (value != this.repairingDefaultLogin)
                {
                    this.repairingDefaultLogin = value;
                    NotifyPropertyChanged("RepairingDefaultLogin");
                }
            }
        }

        public TaskStates RepairingDb
        {
            get { return repairingDb; }
            set
            {
                if (value != this.repairingDb)
                {
                    this.repairingDb = value;
                    NotifyPropertyChanged("RepairingDb");
                }
            }
        }

        public TaskStates RepairingDbObjects
        {
            get { return repairingDbObjects; }
            set
            {
                if (value != this.repairingDbObjects)
                {
                    this.repairingDbObjects = value;
                    NotifyPropertyChanged("RepairingDbObjects");
                }
            }
        }

        public bool RepairInProgress
        {
            get { return repairInProgress; }
            private set
            {
                if (value != this.repairInProgress)
                {
                    this.repairInProgress = value;
                    NotifyPropertyChanged("RepairInProgress");
                }
            }
        }

        public bool RepairComplete
        {
            get { return repairComplete; }
            private set
            {
                if (value != this.repairComplete)
                {
                    this.repairComplete = value;
                    NotifyPropertyChanged("RepairComplete");
                }
            }
        }

        public bool ShowDetails
        {
            get { return showDetails; }
            private set
            {
                if (value != this.showDetails)
                {
                    this.showDetails = value;
                    NotifyPropertyChanged("ShowDetails");
                }
            }
        }

        public bool ShowErrors
        {
            get { return showErrors; }
            private set
            {
                if (value != this.showErrors)
                {
                    this.showErrors = value;
                    NotifyPropertyChanged("ShowErrors");
                }
            }
        }

        public string Message
        {
            get { return message; }
            private set
            {
                if (value != this.message)
                {
                    this.message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        private async void StartRepair(StartUpModel start)
        {
            RepairInProgress = true;
            ShowErrors = false;
            await Task.Factory.StartNew(() =>
                {
                    StartUpModel st = start;

                    CreatingBackup = TaskStates.PerformingTask;
                    if (CreateBackup())
                        CreatingBackup = TaskStates.TaskCompleteSucceeded;
                    else CreatingBackup = TaskStates.TaskCompleteFailed;

                    if (!st.IsSQLInstalled)
                    {
                        RepairingInstallation = TaskStates.PerformingTask;
                        if (RepairInstallation())
                            RepairingInstallation = TaskStates.TaskCompleteSucceeded;
                        else RepairingInstallation = TaskStates.TaskCompleteFailed;
                    }
                    else RepairingInstallation = TaskStates.TaskCompleteSucceeded;

                    if (!st.IsSQLInstanceInstalled)
                    {
                        RepairingInstance = TaskStates.PerformingTask;
                        if (RepairInstance())
                            RepairingInstance = TaskStates.TaskCompleteSucceeded;
                        else RepairingInstance = TaskStates.TaskCompleteFailed;
                    }
                    else RepairingInstance = TaskStates.TaskCompleteSucceeded;

                    if (!st.IsDefaultLoginOK)
                    {
                        RepairingDefaultLogin = TaskStates.PerformingTask;
                        if (RepairInstance())
                            RepairingDefaultLogin = TaskStates.TaskCompleteSucceeded;
                        else
                            RepairingDefaultLogin = TaskStates.TaskCompleteFailed;
                    }
                    else RepairingDefaultLogin = TaskStates.TaskCompleteSucceeded;

                    if (!st.IsDbOK)
                    {
                        RepairingDb = TaskStates.PerformingTask;
                        if (RepairDb())
                            RepairingDb = TaskStates.TaskCompleteSucceeded;
                        else RepairingDb = TaskStates.TaskCompleteSucceeded;
                    }
                    else RepairingDb = TaskStates.TaskCompleteSucceeded;

                    if (!st.IsDbObjectsOK)
                    {
                        RepairingDbObjects = TaskStates.PerformingTask;
                        if (RepairDbObjects())
                            RepairingDbObjects = TaskStates.TaskCompleteSucceeded;
                        else RepairingDbObjects = TaskStates.TaskCompleteFailed;
                    }
                    else RepairingDbObjects = TaskStates.TaskCompleteSucceeded;

                });
            Message = "We have fixed a few errors that were causing UmanyiSMS to function improperly. You can get back to work now." +
                "\r\n\r\n Press 'Continue'";
            RepairInProgress = false;
            ShowDetails = false;
            RepairComplete = true;
        }

        private bool CreateBackup()
        {
            return true;
        }

        private bool RepairInstallation()
        {
            return true;
        }

        private bool RepairInstance()
        {
            return true;
        }

        private bool RepaireDefaultLogin()
        {
            return true;
        }

        private bool RepairDb()
        {
            return true;
        }

        private bool RepairDbObjects()
        {
            return true;
        }

        public override void Reset()
        {
            creatingBackup = TaskStates.Idle;
            repairingInstallation = TaskStates.Idle;
            repairingInstance = TaskStates.Idle;
            repairingDefaultLogin = TaskStates.Idle;
            repairingDb = TaskStates.Idle;
            repairingDbObjects = TaskStates.Idle;
            RepairInProgress = false;
            RepairComplete = false;
            ShowDetails = false;
            ShowErrors = true;
            Message = "";
        }

    }

    
}
