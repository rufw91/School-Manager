

using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SetFeesStructureVM  : ViewModelBase
    {
        CombinedClassModel selectedCombinedClass;
        FeesStructureModel currentStruct;
        FeesStructureEntryModel newEntry;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        private bool saveForAllClasses;
        private ObservableCollection<TermModel> allTerms;
        private TermModel selectedTerm;
        private bool saveForAllTerms;
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
            AllCombinedClasses = await Institution.Controller.DataController.GetAllCombinedClassesAsync();
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
        }

        public ObservableCollection<TermModel> AllTerms
        {
            get { return this.allTerms; }

            private set
            {
                if (value != this.allTerms)
                {
                    this.allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        public TermModel SelectedTerm
        {
            get { return this.selectedTerm; }

            set
            {
                if (value != this.selectedTerm)
                {
                    this.selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
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
                bool succ = true;
                if (saveForAllTerms)
                {
                    currentStruct.StartDate = allTerms[0].StartDate;
                    currentStruct.EndDate = allTerms[2].EndDate;
                }
                else
                {
                    currentStruct.StartDate = selectedTerm.StartDate;
                    currentStruct.EndDate = selectedTerm.EndDate;
                }
                if (saveForAllClasses)
                {
                    foreach (var s in allCombinedClasses)
                        foreach (var c in s.Entries)
                        {
                            currentStruct.ClassID = c.ClassID;
                            succ = succ&&await DataController.SaveNewFeesStructureAsync(currentStruct);
                        }
                }
                else
                {

                    foreach (var c in selectedCombinedClass.Entries)
                    {
                        currentStruct.ClassID = c.ClassID;
                        succ = succ && await DataController.SaveNewFeesStructureAsync(currentStruct);
                    }
                }
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                                MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
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
            return  (saveForAllTerms ? true : selectedTerm != null)&&saveForAllClasses ? currentStruct.Entries.Count > 0 : (selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
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

        public bool SaveForAllTerms
        {
            get { return saveForAllTerms; }
            set
            {
                if (saveForAllTerms != value)
                {
                    saveForAllTerms = value;
                    NotifyPropertyChanged("SaveForAllTerms");
                    SelectedTerm = null;
                }
            }
        }
        
        private async void RefreshEntries()
        {
            CurrentStructure.Entries = (await DataController.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID,DateTime.Now)).Entries;
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
