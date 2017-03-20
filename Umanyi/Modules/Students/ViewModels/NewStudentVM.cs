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
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewStudentVM : ViewModelBase
    {
        StudentModel newStudent;
        ObservableCollection<ClassModel> allClasses;

        public NewStudentVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }

        protected override async void InitVars()
        {
            Title = "NEW STUDENT";

            newStudent = new StudentModel();
            
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            newStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="StudentID")
                        newStudent.CheckErrors();
                };
            newStudent.StudentID = await MySystem.Controller.DataController.GetNewID("dbo.Student");
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o => 
            {
                bool succ = await DataController.SaveNewStudentAsync(newStudent); 
                if (succ)
                    Reset();
                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details.", succ ? "Success" : "Error", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
            }, o => 
                ValidateStudent());
            ClearImageCommand = new RelayCommand(o => { newStudent.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStudent.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
           
        }
        

        public override void Reset()
        {
            newStudent.Reset();
            
        }

        public Array GenderValues
        {
            get { return Enum.GetValues(typeof(Gender)); }
        }
        
        public StudentModel NewStudent
        { get { return newStudent; } }
        
        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            set
            {
                if (value != allClasses)                
                    allClasses = value;
                NotifyPropertyChanged("AllClasses");
            }
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
                      && (newStudent.DateOfBirth != null) && (newStudent.DateOfAdmission != null)
                      && (EmailValidator.IsValidEmail(newStudent.Email))
                      && newStudent.ClassID>0;
            return isOk&&!newStudent.HasErrors;
        }


       
    }
}
