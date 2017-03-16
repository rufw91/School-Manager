using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewStudentVM : ViewModelBase
    {
        
        StudentModel newStudent;
        ObservableCollection<DormModel> allDorms;
        ObservableCollection<ClassModel> allClasses;
        private Boardingtype boardingValue;

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
            BoardingValue = Boardingtype.Boarder;
            AllDorms = await DataAccess.GetAllDormsAsync();
            AllClasses = await DataAccess.GetAllClassesAsync();
            newStudent.PropertyChanged += (o, e) =>
                {
                    if ((e.PropertyName=="BedNo")||(e.PropertyName=="StudentID"))
                        newStudent.CheckErrors();
                };
            newStudent.IsBoarder = true;
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o => 
            {
                bool succ = await DataAccess.SaveNewStudentAsync(newStudent); 
                if (succ)
                    Reset();
                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details.", succ ? "Success" : "Error", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
            }, o => 
                ValidateStudent());
            ClearImageCommand = new RelayCommand(o => { newStudent.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStudent.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
            ClearDormCommand = new RelayCommand(o => { newStudent.DormitoryID = 0; }, o => true);
           
        }
        

        public override void Reset()
        {
            newStudent.Reset();

        }

        public Array BoardingValues
        {
            get { return Enum.GetValues(typeof(Boardingtype)); }
        }

        public Array GenderValues
        {
            get { return Enum.GetValues(typeof(Gender)); }
        }

        public Boardingtype BoardingValue
        {
            get { return boardingValue; }
            set
            {
                if (value != boardingValue)
                {
                    boardingValue = value;
                    newStudent.IsBoarder = boardingValue == Boardingtype.Boarder ? true : false;
                }

                NotifyPropertyChanged("BoardingValue");
            }
        }

        public StudentModel NewStudent
        { get { return newStudent; } }

        public ObservableCollection<DormModel> AllDorms
        {
            get { return allDorms; }
            set
            {
                if (value != allDorms)
                    allDorms = value;
                NotifyPropertyChanged("AllDorms");
            }
        }

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
