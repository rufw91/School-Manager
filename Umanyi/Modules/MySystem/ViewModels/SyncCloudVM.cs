using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class SyncCloudVM: ViewModelBase
    {
        private bool v1, v2, v3, v4, v5, v6;
        private decimal v7,v8;
        private bool vx;
        public SyncCloudVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "SYNC CLOUD";
            SyncStarted = false;
        }

        protected override void CreateCommands()
        {
            SyncCommand = new RelayCommand( o => {
                SyncStarted = true;
                IsBusy = true; 
                //await SyncHelper.Sync(new Progress<SyncOperationProgress>(DisplayProgress)); 
                IsBusy = false;
                if (!IsSyncSucceeded)
                    MessageBox.Show("Sync did not complete successfully. Check your network connection and try again.", 
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show("Successfully completed sync.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }, o => !IsBusy);
        }

        public bool SyncStarted
        {
            get { return vx; }
            set
            {
                if (value != this.vx)
                {
                    this.vx = value;
                    NotifyPropertyChanged("SyncStarted");
                }
            }
        }

        public bool IsPrepareActive
        {
            get { return v1; }
            set
            {
                if (value != this.v1)
                {
                    this.v1 = value;
                    NotifyPropertyChanged("IsPrepareActive");
                }
            }
        }

        public bool IsPrepareSucceeded
        {
            get { return v2; }
            set
            {
                if (value != this.v2)
                {
                    this.v2 = value;
                    NotifyPropertyChanged("IsPrepareSucceeded");
                }
            }
        }

        public bool IsPrepareCompleted
        {
            get { return v3; }
            set
            {
                if (value != this.v3)
                {
                    this.v3 = value;
                    NotifyPropertyChanged("IsPrepareCompleted");
                }
            }
        }

        public bool IsSyncActive
        {
            get { return v4; }
            set
            {
                if (value != this.v4)
                {
                    this.v4 = value;
                    NotifyPropertyChanged("IsSyncActive");
                }
            }
        }

        public bool IsSyncSucceeded
        {
            get { return v5; }
            set
            {
                if (value != this.v5)
                {
                    this.v5 = value;
                    NotifyPropertyChanged("IsSyncSucceeded");
                }
            }
        }

        public bool IsSyncCompleted
        {
            get { return v6; }
            set
            {
                if (value != this.v6)
                {
                    this.v6 = value;
                    NotifyPropertyChanged("IsSyncCompleted");
                }
            }
        }

        public decimal PrepareProgress
        {
            get { return v7; }
            set
            {
                if (value != this.v7)
                {
                    this.v7 = value;
                    NotifyPropertyChanged("PrepareProgress");
                }
            }
        }

        public decimal SyncProgress
        {
            get { return v8; }
            set
            {
                if (value != this.v8)
                {
                    this.v8 = value;
                    NotifyPropertyChanged("SyncProgress");
                }
            }
        }

        public void DisplayProgress(SyncOperationProgress ms)
        {
            switch (ms.CurrentItem)
            {
                case 1: 
                    IsPrepareActive = true;
                    IsSyncActive = false;
                    IsPrepareCompleted = ms.Completed;
                    IsPrepareSucceeded = ms.Succeeded; 
                    PrepareProgress = ms.Percentage;
                    if (IsPrepareCompleted)
                        IsPrepareActive = false;
                        break;
                case 2:
                    IsSyncActive = true;
                    IsSyncCompleted = ms.Completed;
                    IsSyncSucceeded = ms.Succeeded;
                    SyncProgress = ms.Percentage;
                    if (IsSyncCompleted)
                        IsSyncActive = false;
                    break;
            }
        }

        public ICommand SyncCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }
    }
}
