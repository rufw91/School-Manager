using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ClassSubjectSetupVM : ViewModelBase
    {
        SubjectsSetupModel subjectsSetup;
        ClassesSetupModel classesSetup;

        ClassesSetupEntryModel newClass;
        SubjectsSetupEntryModel newSubject;
        int selectedClassID;

        public ClassSubjectSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override async void InitVars()
        {
            Title = "CLASS & SUBJECT SETUP";
            subjectsSetup = new SubjectsSetupModel();
            classesSetup = new ClassesSetupModel();
            newClass = new ClassesSetupEntryModel();
            newSubject = new SubjectsSetupEntryModel();
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
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
                    NewSubject = new SubjectsSetupEntryModel();
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

        public int SelectedClassID
        {
            get { return selectedClassID; }

            set
            {
                selectedClassID = value;
                if (selectedClassID == subjectsSetup.ClassID)
                    return;
                else
                {
                    NotifyPropertyChanged("SelectedClassID");
                    subjectsSetup.ClassID = selectedClassID;
                    RefreshSubjectEntries();
                }
            }
        }
        private async void RefreshSubjectEntries()
        {
            if (selectedClassID==0)
            {
                subjectsSetup.Entries.Clear();
                subjectsSetup.ClassID = 0;
                return;
            }   
            subjectsSetup.Entries.Clear();
            var temp= await DataAccess.GetSubjectsRegistredToClassAsync(selectedClassID);
            foreach (SubjectModel sm in temp)
                subjectsSetup.Entries.Add(new SubjectsSetupEntryModel(sm));
        }

        private async void RefreshClassEntries()
        {
            classesSetup.Entries.Clear();
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
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
            return subjectsSetup.ClassID>0
                && !string.IsNullOrWhiteSpace(newSubject.NameOfSubject)
                && newSubject.MaximumScore > 0;
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
