using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewStudentVM : ViewModelBase
    {
        StudentModel newStudent;
        ObservableCollection<DormModel> allDorms;
        ObservableCollection<ClassModel> allClasses;
        public NewStudentVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o => 
            {
                bool succ = await DataAccess.SaveNewStudentAsync(newStudent,false); 
                if (succ)
                    Reset(); 
            }, o => 
                ValidateStudent());
            ClearImageCommand = new RelayCommand(o => { newStudent.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStudent.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
            ClearDormCommand = new RelayCommand(o => { newStudent.DormitoryID = null; }, o => true);
            ImportFromExcelCommand = new RelayCommand(o=> 
            {
                if (ShowImportWindowAction != null)
                    ShowImportWindowAction.Invoke();
            });
        }

        protected override async void InitVars()
        {            
            Title = "NEW STUDENT";
            newStudent = new StudentModel();
            AllDorms = await DataAccess.GetAllDormsAsync();
            NotifyPropertyChanged("AllDorms");
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }

        public override void Reset()
        {
            newStudent.Reset();
        }

        public StudentModel NewStudent
        { get { return newStudent; } }

        public ObservableCollection<DormModel> AllDorms
        {
            get { return allDorms; }
            private set
            {
                if (value != allDorms)
                    allDorms = value;
            }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            private set
            {
                if (value != allClasses)                
                    allClasses = value;
            }
        }

        public Action ShowImportWindowAction
        {
            get;
            set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public ICommand ClearImageCommand
        {
            get;
            private set;
        }

        public ICommand ImportFromExcelCommand
        {
            get;
            private set;

        }

        public ICommand BrowseCommand
        {
            get;
            private set;
        }
        public ICommand ClearDormCommand
        {
            get;
            private set;
        }

        private bool ValidateStudent()
        {
            bool isOk = !string.IsNullOrWhiteSpace(newStudent.FirstName) && !string.IsNullOrWhiteSpace(newStudent.LastName)
                && !string.IsNullOrWhiteSpace(newStudent.NameOfGuardian) && !string.IsNullOrWhiteSpace(newStudent.Email)
                   && !string.IsNullOrWhiteSpace(newStudent.GuardianPhoneNo) && !string.IsNullOrWhiteSpace(newStudent.Address)
                    && !string.IsNullOrWhiteSpace(newStudent.City) && !string.IsNullOrWhiteSpace(newStudent.PostalCode)
                      && !(newStudent.DateOfBirth == null) && !(newStudent.DateOfAdmission == null)
                      && (EmailValidator.IsValidEmail(newStudent.Email))
                      && newStudent.ClassID>0;
            return isOk;
        }

    }
}
