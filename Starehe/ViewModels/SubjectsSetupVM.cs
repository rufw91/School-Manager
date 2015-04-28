using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class SubjectsSetupVM:ViewModelBase
    {
        SubjectsSetupModel subjectsSetup;
        SubjectsSetupEntryModel newSubject;
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;

        public SubjectsSetupVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "SUBJECTS SETUP";
            subjectsSetup = new SubjectsSetupModel();
            newSubject = new SubjectsSetupEntryModel();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();

            PropertyChanged += async(o, e) =>
                {
                    if (e.PropertyName == "SelectedCombinedClass")
                    {
                        if ((selectedCombinedClass == null) || (selectedCombinedClass.Entries == subjectsSetup.Classes))
                            return;
                        else
                        {
                            if (selectedCombinedClass != null)
                                subjectsSetup.Classes = selectedCombinedClass.Entries;
                            await RefreshSubjectEntries();
                        }
                    }
                };
            IsBusy = false;
        }

        public SubjectsSetupModel SubjectsSetup
        {
            get { return this.subjectsSetup; }
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            private set
            {
                if (allCombinedClasses != value)
                {
                    allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public SubjectsSetupEntryModel NewSubject
        {
            get { return this.newSubject; }

            set
            {
                if (value != newSubject)
                {
                    newSubject = value;
                    NotifyPropertyChanged("NewSubject");
                }
            }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }

            set
            {
                if (selectedCombinedClass != value)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                }                
            }
        }
        private async Task RefreshSubjectEntries()
        {
            subjectsSetup.Entries.Clear();
            if (selectedCombinedClass == null || selectedCombinedClass.Entries.Count == 0)
                return;
            var temp = await DataAccess.GetSubjectsRegistredToClassAsync(selectedCombinedClass.Entries[0].ClassID);
            foreach (SubjectModel sm in temp)
                subjectsSetup.Entries.Add(new SubjectsSetupEntryModel(sm) { SubjectSetupID = 1 });
        }

        protected override void CreateCommands()
        {
            AddNewSubjectCommand = new RelayCommand(async o =>
            {
                subjectsSetup.Entries.Add(NewSubject);
                bool succ = await DataAccess.SaveNewSubjectSetupAsync(subjectsSetup);
                if (succ)
                {
                    NewSubject = new SubjectsSetupEntryModel();
                    SelectedCombinedClass = null;
                }
            }, o => CanAddSubject());
        }

        private bool CanAddSubject()
        {
            bool temp = subjectsSetup.Classes.Count > 0
                && !string.IsNullOrWhiteSpace(newSubject.NameOfSubject)
                && newSubject.MaximumScore > 0;
            return temp;
        }

        public ICommand AddNewSubjectCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }
    }
}
