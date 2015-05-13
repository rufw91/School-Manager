﻿using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SetFeesStructureVM  : ViewModelBase
    {
        CombinedClassModel selectedCombinedClass;
        FeesStructureModel currentStruct;
        FeesStructureEntryModel newEntry;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        private bool saveForAllClasses;
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
            SaveForAllClasses = false;
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return this.allCombinedClasses; }

            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
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
                if (saveForAllClasses)
                {
                    foreach (var s in allCombinedClasses)
                        foreach (var c in s.Entries)
                        {
                            currentStruct.ClassID = c.ClassID;
                            await DataAccess.SaveNewFeesStructureAsync(currentStruct);
                        }
                }
                else
                {

                    foreach (var c in selectedCombinedClass.Entries)
                    {
                        currentStruct.ClassID = c.ClassID;
                        await DataAccess.SaveNewFeesStructureAsync(currentStruct);
                    }
                }
                this.Reset();
            }, o => CanSave());
        }

        private bool CanAddNewEntry()
        {
            return (!string.IsNullOrWhiteSpace(newEntry.Name)
                && newEntry.Amount > 0) && (saveForAllClasses ? true :
                (selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0));
        }

        public override void Reset()
        {
            SelectedCombinedClass = null;
            currentStruct.Reset();
            newEntry.Reset();
        }

        private bool CanSave()
        {
            return saveForAllClasses ? currentStruct.Entries.Count > 0 : (selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
                currentStruct.Entries.Count > 0);
        }

        public FeesStructureModel CurrentStructure
        {
            get { return currentStruct; }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }
            set
            {
                selectedCombinedClass = value;
                NotifyPropertyChanged("SelectedCombinedClass");
                if (selectedCombinedClass != null && selectedCombinedClass.Entries.Count>0)
                    RefreshEntries();
            }
        }

        public bool SaveForAllClasses
        {
            get { return saveForAllClasses; }
            set
            {
                if (saveForAllClasses != value)
                {
                    saveForAllClasses = value;
                    NotifyPropertyChanged("SaveForAllClasses");
                    SelectedCombinedClass = null;
                }
            }
        }
        
        private async void RefreshEntries()
        {
            CurrentStructure.Entries = (await DataAccess.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID,DateTime.Now)).Entries;
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
