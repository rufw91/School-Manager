using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SetFeesStructureVM  : ViewModelBase
    {
        int currentClassID;
        FeesStructureModel currentStruct;
        FeesStructureEntryModel newEntry;
        ObservableCollection<ClassModel> allClasses;
        public SetFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "SET FEES STRUCTURE";
            currentStruct = new FeesStructureModel();
            newEntry = new FeesStructureEntryModel();
            CurrentClassID = 0;
            AllClasses = await DataAccess.GetAllClassesAsync();
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return this.allClasses; }

            private set
            {
                if (value != this.allClasses)
                {
                    this.allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        protected override void CreateCommands()
        {
            AddEntryCommand = new RelayCommand(o => 
            { 
                CurrentStructure.Entries.Add(newEntry); 
                NewEntry = new FeesStructureEntryModel();
            },
                o => CanAddNewEntry());
            SaveCommand = new RelayCommand(async o =>
            {
                await DataAccess.SaveNewFeesStructureAsync(currentStruct);
                this.Reset();
            }, o => CanSave());
        }

        private bool CanAddNewEntry()
        {
            return !string.IsNullOrWhiteSpace(newEntry.Name)
                && newEntry.Amount > 0
                && currentClassID > 0;
        }

        public override void Reset()
        {
            CurrentClassID = 0;
            currentStruct.Reset();
            newEntry.Reset();
        }

        private bool CanSave()
        {
            return currentStruct.ClassID>0 &&
                currentStruct.Entries.Count > 0;
        }

        public FeesStructureModel CurrentStructure
        {
            get { return currentStruct; }
        }

        public int CurrentClassID
        {
            get { return currentClassID; }
            set
            {
                currentClassID = value;
                NotifyPropertyChanged("CurrentClassID");
                currentStruct.ClassID = currentClassID;
                if (currentClassID>0)
                    RefreshEntries();
            }
        }
        
        private async void RefreshEntries()
        {
            CurrentStructure.Entries = (await DataAccess.GetFeesStructureAsync(currentClassID,DateTime.Now)).Entries;
        }

        public FeesStructureEntryModel NewEntry
        {
            get { return newEntry; }
            set
            {
                if (value != newEntry)
                {
                    newEntry = value;
                    NotifyPropertyChanged("NewEntry");
                }
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand AddEntryCommand
        {
            get;
            private set;
        }
    }
}
