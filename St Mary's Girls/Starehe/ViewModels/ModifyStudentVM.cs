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
        public ModifyStudentVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "MODIFY STUDENT";
            NewStudent = new ModifyStudentModel();
            AllDorms = new ObservableCollection<DormModel>();
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
                    MessageBox.Show("Succesfully updated employee.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this Time.\r\n Error: Database Access Failure.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                Reset();
                IsBusy = false;
            }, o => !IsBusy && CanSave());
            ClearImageCommand = new RelayCommand(o => { newStudent.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStudent.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
            ClearDormCommand = new RelayCommand(o => { newStudent.DormitoryID = null; }, o => true);

        }

        private bool CanSave()
        {
            newStudent.CheckErrors();
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
