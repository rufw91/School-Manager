using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Principal")]
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
            newStaff.StaffID = await GetNewID();
            IsBusy = false;
        }

        private Task<int> GetNewID()
        {
            return Task.Run<int>(() =>
            {
                string selectStr = "declare @newid int;  set @newid =dbo.GetNewID('Institution.Staff');  select @newid;";

                string finalStr = DataAccessHelper.ExecuteScalar(selectStr);
                int res;
                int.TryParse(finalStr, out res);
                return res;
            });
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                SecurePassword.MakeReadOnly();
                SqlCredential c = new SqlCredential(newStaff.StaffID + "", SecurePassword);

                bool succ = await DataAccess.SaveNewStaffAsync(newStaff);
                if (canSaveUser)
                    succ = succ && await UsersHelper.CreateNewUserAsync(c, Role, newStaff.Name,newStaff.SPhoto);
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

        public ObservableCollection<UserRole> AllRoles
        {
            get { return UsersHelper.GetUserRolesForDisplay(); }
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
                    CanSaveUser = !(role == UserRole.None);
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
            bool isOk = newStaff.StaffID>0&&
                !string.IsNullOrWhiteSpace(newStaff.Name) && !string.IsNullOrWhiteSpace(newStaff.NationalID)
                && !string.IsNullOrWhiteSpace(newStaff.Email) && !(newStaff.DateOfAdmission == null)
                   && !string.IsNullOrWhiteSpace(newStaff.PhoneNo) && !string.IsNullOrWhiteSpace(newStaff.Address)
                    && !string.IsNullOrWhiteSpace(newStaff.City) && !string.IsNullOrWhiteSpace(newStaff.PostalCode)
                      && (EmailValidator.IsValidEmail(newStaff.Email));
            return isOk;
        }
    }
}
