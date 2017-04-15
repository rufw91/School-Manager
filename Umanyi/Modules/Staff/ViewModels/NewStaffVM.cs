using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Staff.Models;
using UmanyiSMS.Modules.Staff.Controller;

namespace UmanyiSMS.Modules.Staff.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class NewStaffVM : ViewModelBase
    {
        StaffModel newStaff;
        private UserRole role;
        private bool canSaveUser;
        public NewStaffVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            newStaff = new StaffModel();
            Title = "NEW STAFF MEMBER";
            IsBusy = true;
            newStaff.StaffID = await MySystem.Controller.DataController.GetNewID("dbo.Staff");
            newStaff.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "StaffID")
                        newStaff.CheckErrors();
                };
            IsBusy = false;
        }
                
        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;


                bool succ = await DataController.SaveNewStaffAsync(newStaff);
                if (canSaveUser)
                {
                    SecurePassword.MakeReadOnly();
                    SqlCredential c = new SqlCredential(newStaff.StaffID + "", SecurePassword);
                    succ = succ && await UsersHelper.CreateNewUserAsync(c, Role, newStaff.Name, newStaff.SPhoto);
                }
                if (succ)
                {
                    MessageBox.Show("Succesfully saved staff member.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this time.\r\n Error: Database Access Failure.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                if (ResetAction != null)
                    ResetAction.Invoke();
                IsBusy = false;
            }, o => !IsBusy && ValidateStaff());

            ClearImageCommand = new RelayCommand(o => { newStaff.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStaff.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
        }

        internal Action ResetAction
        { get; set; }

        internal SecureString SecurePassword
        { get; set; }

        public Array AllRoles
        {
            get { return Enum.GetValues(typeof(UserRole)); }
        }

        public UserRole Role
        {
            get { return this.role; }

            set
            {
                if (value != this.role)
                {
                    this.role = value;
                    NotifyPropertyChanged("Role");
                    CanSaveUser = role > UserRole.None;
                }
            }
        }

        public bool CanSaveUser
        {
            get { return this.canSaveUser; }

            set
            {
                if (value != this.canSaveUser)
                {
                    this.canSaveUser = value;
                    NotifyPropertyChanged("CanSaveUser");
                }
            }
        }

        public StaffModel NewStaff
        {
            get { return newStaff; }
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
        public override void Reset()
        {
            newStaff.Reset();
            Role = UserRole.None;
        }
        private bool ValidateStaff()
        {
            
            bool isOk = newStaff.StaffID>0&&!newStaff.HasErrors&&
                !string.IsNullOrWhiteSpace(newStaff.Name) && !string.IsNullOrWhiteSpace(newStaff.NationalID)
                && !string.IsNullOrWhiteSpace(newStaff.Email) && !(newStaff.DateOfAdmission == null)
                   && !string.IsNullOrWhiteSpace(newStaff.PhoneNo) && !string.IsNullOrWhiteSpace(newStaff.Address)
                    && !string.IsNullOrWhiteSpace(newStaff.City) && !string.IsNullOrWhiteSpace(newStaff.PostalCode)
                      && (EmailValidator.IsValidEmail(newStaff.Email)) && !string.IsNullOrWhiteSpace(newStaff.Designation);

            if (canSaveUser)
                if (SecurePassword == null)
                    isOk = false;
            return isOk;
        }
    }
}
