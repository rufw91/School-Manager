using Helper.Models;
using System;
using Helper;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ModifyStudentVM : ViewModelBase
    {
        ModifyStudentModel newStudent;
        ObservableCollection<DormModel> allDorms;
        ObservableCollection<ClassModel> allClasses;
        private Boardingtype boardingValue;
        private bool isStudentInactive;
        public ModifyStudentVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            IsStudentInactive = false;
            Title = "MODIFY STUDENT";
            BoardingValue = Boardingtype.Boarder;
            NewStudent = new ModifyStudentModel();
            newStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        newStudent.CheckErrors();
                        if (!newStudent.HasErrors)
                            IsStudentInactive = !newStudent.IsActive;
                        

                        BoardingValue = newStudent.DormitoryID > 0 ? Boardingtype.Boarder : Boardingtype.DayScholar;
                    }
                };
            AllDorms = await DataAccess.GetAllDormsAsync();
            AllClasses = await DataAccess.GetAllClassesAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;

                bool succ = await DataAccess.UpdateStudentAsync(newStudent);
                if (succ)
                {
                    MessageBox.Show("Succesfully updated details.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this Time.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                Reset();
                IsBusy = false;
            }, o => !IsBusy && CanSave());
            ClearImageCommand = new RelayCommand(o => { newStudent.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStudent.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
            ClearDormCommand = new RelayCommand(o => { newStudent.DormitoryID = 0; }, o => true);
            MakeActiveCommand = new RelayCommand(o => 
            {
                IsStudentInactive = false;
                IsBusy = true;
                DataAccess.SetStudentActiveAsync(newStudent);
                IsBusy = false;
            }, o => !newStudent.HasErrors && isStudentInactive);
            IgnoreCommand = new RelayCommand(o => { IsStudentInactive = false; }, o => !newStudent.HasErrors && isStudentInactive);
        }

        private bool CanSave()
        {
            return !newStudent.HasErrors && ValidateStudent();
        }

        private bool ValidateStudent()
        {
            bool isOk = !string.IsNullOrWhiteSpace(newStudent.FirstName) && !string.IsNullOrWhiteSpace(newStudent.LastName)
                && !string.IsNullOrWhiteSpace(newStudent.NameOfGuardian) && !string.IsNullOrWhiteSpace(newStudent.Email)
                   && !string.IsNullOrWhiteSpace(newStudent.GuardianPhoneNo) && !string.IsNullOrWhiteSpace(newStudent.Address)
                    && !string.IsNullOrWhiteSpace(newStudent.City) && !string.IsNullOrWhiteSpace(newStudent.PostalCode)
                      && !(newStudent.DateOfBirth == null) && !(newStudent.DateOfAdmission == null)
                      && (EmailValidator.IsValidEmail(newStudent.Email))
                      && newStudent.ClassID > 0;
            return isOk;
        }

        public Array BoardingValues
        {
            get { return Enum.GetValues(typeof(Boardingtype)); }
        }

        public bool IsStudentInactive
        {
            get { return isStudentInactive; }
            set
            {
                if (value != isStudentInactive)
                {
                    isStudentInactive = value;
                    NotifyPropertyChanged("IsStudentInactive");
                }
            }
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

        public ObservableCollection<DormModel> AllDorms
        {
            get { return allDorms; }
            private set
            {
                if (value != allDorms)
                {
                    allDorms = value;
                    NotifyPropertyChanged("AllDorms");
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

        public ICommand MakeActiveCommand
        {
            get;
            private set;
        }
        public ICommand IgnoreCommand
        { get; private set; }
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
        public ICommand SaveCommand
        { get; private set; }

        public ModifyStudentModel NewStudent
        {
            get { return this.newStudent; }

            private set
            {
                if (value != this.newStudent)
                {
                    this.newStudent = value;
                    NotifyPropertyChanged("NewStudent");
                }
            }
        }

        public override void Reset()
        {
            NewStudent.Reset();
        }
    }
}
