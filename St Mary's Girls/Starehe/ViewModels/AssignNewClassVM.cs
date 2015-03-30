using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class AssignNewClassVM: ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        StudentSelectModel selectedStudent;
        int selectedClassID;
        private ObservableCollection<ClassModel> allClasses;
        private int newClassID;
        private ClassModel currentClass;
        public AssignNewClassVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ASSIGN NEW CLASS";
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName=="StudentID")
                        selectedStudent.CheckErrors();
                    if (!selectedStudent.HasErrors)
                        CurrentClass = await DataAccess.GetClassAsync(await DataAccess.GetClassIDFromStudentID(selectedStudent.StudentID));
                };

            SelectedClassID = 0;
            NewClassID = 0;
            IsInStudentMode = true;
            AllClasses = await DataAccess.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
             {
                 bool succ = false;
                 if (isInStudentMode)
                 succ = await DataAccess.AssignStudentNewClass(selectedStudent.StudentID, newClassID);
                 else
                     succ = await DataAccess.AssignClassNewClass(selectedClassID, newClassID);
                 MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                 if (succ)
                     Reset();
             }, o => CanSave());
        }

        private bool CanSave()
        {
            return isInStudentMode ? (selectedStudent != null && selectedStudent.StudentID > 0 && !selectedStudent.HasErrors && newClassID > 0) :
                (selectedClassID > 0 && newClassID > 0);
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
