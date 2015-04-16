using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ClassesSetupVM : ViewModelBase
    {
        SubjectsSetupModel subjectsSetup;
        ClassesSetupModel classesSetup;

        ClassesSetupEntryModel newClass;
        SubjectsSetupEntryModel newSubject;
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;

        public ClassesSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override async void InitVars()
        {
            IsBusy = true;
            Title = "CLASSES SETUP";
            subjectsSetup = new SubjectsSetupModel();
            classesSetup = new ClassesSetupModel();
            newClass = new ClassesSetupEntryModel();
            newSubject = new SubjectsSetupEntryModel();
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            IsBusy = false;
        }
        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            set
            {
                if (allCombinedClasses != value)
                {
                    allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }
        protected override void CreateCommands()
        {
            AddNewClassCommand = new RelayCommand(async o =>
            {
                classesSetup.Entries.Add(newClass);
                bool succ = await DataAccess.SaveNewClassSetupAsync(classesSetup);
                if (succ)
                    RefreshClassEntries();
                else
                    classesSetup.Entries.Remove(newClass);
                
                NewClass = new ClassesSetupEntryModel();
            }, o => CanAddClass());

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

        public ClassesSetupModel ClassesSetup
        {
            get { return this.classesSetup; }
        }

        public SubjectsSetupModel SubjectsSetup
        {
            get { return this.subjectsSetup; }
        }

        public ClassesSetupEntryModel NewClass
        {
            get { return this.newClass; }

            set
            {
                if (value != newClass)
                {
                    newClass = value;
                    NotifyPropertyChanged("NewClass");
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
                if ((selectedCombinedClass == null) || (selectedCombinedClass.Entries == subjectsSetup.Classes))
                    return;
                else
                {
                    if (selectedCombinedClass != null)
                        subjectsSetup.Classes = selectedCombinedClass.Entries;
                    RefreshSubjectEntries();
                }
            }
        }
        private async void RefreshSubjectEntries()
        {
            subjectsSetup.Entries.Clear();
            if (selectedCombinedClass == null || selectedCombinedClass.Entries.Count == 0)
                return;            
            var temp= await DataAccess.GetSubjectsRegistredToClassAsync(selectedCombinedClass.Entries[0].ClassID);
            foreach (SubjectModel sm in temp)
                subjectsSetup.Entries.Add(new SubjectsSetupEntryModel(sm));
        }

        private async void RefreshClassEntries()
        {
            classesSetup.Entries.Clear();
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
        }

        public ICommand AddNewClassCommand
        {
            get;
            private set;
        }

        public ICommand AddNewSubjectCommand
        {
            get;
            private set;
        }
        
        private bool CanAddSubject()
        {
            bool temp= subjectsSetup.Classes.Count>0
                && !string.IsNullOrWhiteSpace(newSubject.NameOfSubject)
                && newSubject.MaximumScore > 0;
            Debug.WriteLine("CanAddSubject: " + temp);
            Debug.WriteLine("HasClasses: " + (subjectsSetup.Classes.Count > 0));
            return temp;
        }

        private bool CanAddClass()
        {
            return !string.IsNullOrWhiteSpace(newClass.NameOfClass);
        }

        public override void Reset()
        {
            
        }
    }
}
