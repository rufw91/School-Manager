using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewProjectVM : ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        StudentSelectModel selectedStudent;
        int selectedClassID;
        private ObservableCollection<ClassModel> allClasses;
        private ObservableCollection<int> allYears;
        private int newClassID;
        private ClassModel currentClass;
        private int currYear;
        private int prevYear;
        public NewProjectVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ASSIGN NEW CLASS";
            allYears = new ObservableCollection<int>();
            for (int i = 2014; i < 2024; i++)
                allYears.Add(i);
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName=="StudentID")
                        selectedStudent.CheckErrors();
                    if (!selectedStudent.HasErrors)
                        CurrentClass = await Institution.Controller.DataController.GetClassAsync(await DataController.GetClassIDFromStudentID(selectedStudent.StudentID));
                };

            SelectedClassID = 0;
            NewClassID = 0;
            IsInStudentMode = true;
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            CurrYear = DateTime.Now.Year;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
             {
                 bool succ = false;
                 if (isInStudentMode)
                     succ = await DataController.AssignStudentNewClass(selectedStudent.StudentID, newClassID, prevYear, currYear);
                 else
                     succ = await DataController.AssignClassNewClass(selectedClassID, newClassID, prevYear, currYear);
                 MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                 if (succ)
                     Reset();
             }, o => CanSave());
        }

        private bool CanSave()
        {
            return isInStudentMode ? (selectedStudent != null && selectedStudent.StudentID > 0 && !selectedStudent.HasErrors && newClassID > 0&&prevYear!=currYear) :
                (selectedClassID > 0 && newClassID > 0 && prevYear != currYear && prevYear > 0);
        }

        public bool IsInStudentMode
        {
            get { return isInStudentMode; }

            set
            {
                if (value != isInStudentMode)
                {
                    isInStudentMode = value;
                    NotifyPropertyChanged("IsInStudentMode"); 
                    selectedStudent.Reset();
                }
            }
        }

        public ObservableCollection<int> AllYears
        {
            get { return allYears; }
        }

        public int PrevYear
        {
            get { return prevYear; }

            set
            {
                if (value != prevYear)
                {
                    prevYear = value;
                    NotifyPropertyChanged("PrevYear");
                }
            }
        }

        public int CurrYear
        {
            get { return currYear; }

            set
            {
                if (value != currYear)
                {
                    currYear = value;
                    NotifyPropertyChanged("CurrYear");
                }
            }
        }

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    selectedClassID = 0;
                }
            }
        }
        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }

            set
            {
                if (value != selectedStudent)
                {
                    selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }
        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }

            private set
            {
                if (value != allClasses)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }
        public int NewClassID
        {
            get { return newClassID; }

            private set
            {
                if (value != newClassID)
                {
                    newClassID = value;
                    NotifyPropertyChanged("NewClassID");
                }
            }
        }
        public int SelectedClassID
        {
            get { return selectedClassID; }

            private set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }
        public ClassModel CurrentClass
        {
            get { return currentClass; }

            private set
            {
                if (value != currentClass)
                {
                    currentClass = value;
                    NotifyPropertyChanged("CurrentClass");
                }
            }
        }
        public ICommand SaveCommand
        { get; private set; }
        public override void Reset()
        {
            selectedStudent.Reset();
            NewClassID = 0;
            SelectedClassID = 0;
        }

        
    }
}
