﻿using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Staff.Controller;
using UmanyiSMS.Modules.Staff.Models;

namespace UmanyiSMS.Modules.Staff.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ModifyStaffVM : ViewModelBase
    {
        ModifyStaffModel newStaff;
        private UserRole? role;
        public ModifyStaffVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            IsBusy = true;
            Title = "MODIFY STAFF DETAILS";
            NewStaff = new ModifyStaffModel();
            IsBusy = false;
            newStaff.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "StaffID")
                    {
                        newStaff.CheckErrors();
                        if (!newStaff.HasErrors)
                            Role = await UsersHelper.GetUserRole(newStaff.StaffID.ToString());
                    }
                };
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;

                bool succ = await DataController.UpdateStaffAsync(newStaff);
                if (succ)
                {
                    MessageBox.Show("Succesfully updated staff.", "", MessageBoxButton.OK,
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
            }, o => !IsBusy&&CanSave());
            ClearImageCommand = new RelayCommand(o => { newStaff.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStaff.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
           
        }

        private bool CanSave()
        {            
            return !newStaff.HasErrors && ValidateStaff();
        }
        
        internal Action ResetAction
        { get; set; }

        internal SecureString SecurePassword
        { get; set; }

        public Array AllRoles
        {
            get { return Enum.GetValues(typeof(UserRole)); }
        }

        public UserRole? Role
        {
            get { return this.role; }

            set
            {
                if (value != this.role)
                {
                    this.role = value;
                    NotifyPropertyChanged("Role");
                }
            }
        }

        public ModifyStaffModel NewStaff
        {
            get { return newStaff; }

            private set
            {
                if (value != this.newStaff)
                {
                    this.newStaff = value;
                    NotifyPropertyChanged("NewStaff");
                }
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

        private bool ValidateStaff()
        {
            bool isOk = newStaff.StaffID > 0
                && !string.IsNullOrWhiteSpace(newStaff.Name) && !string.IsNullOrWhiteSpace(newStaff.NationalID)
                && !string.IsNullOrWhiteSpace(newStaff.Email) && !(newStaff.DateOfAdmission == null)
                   && !string.IsNullOrWhiteSpace(newStaff.PhoneNo) && !string.IsNullOrWhiteSpace(newStaff.Address)
                    && !string.IsNullOrWhiteSpace(newStaff.City) && !string.IsNullOrWhiteSpace(newStaff.PostalCode)
                      && (EmailValidator.IsValidEmail(newStaff.Email)) && !string.IsNullOrWhiteSpace(newStaff.Designation);
            return isOk;
        }

        public override void Reset()
        {
            NewStaff.Reset();
            Role = UserRole.None;
        }
    }
}
