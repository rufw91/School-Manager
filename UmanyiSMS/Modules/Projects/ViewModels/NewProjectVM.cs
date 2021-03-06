﻿using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewProjectVM : ViewModelBase
    {
        private ProjectModel newProject;

        public ProjectModel NewProject
        {
            get
            {
                return this.newProject;
            }
        }

        public System.Windows.Input.ICommand SaveCommand
        {
            get;
            private set;
        }

        public NewProjectVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override void InitVars()
        {
            base.Title = "NEW PROJECT";
            this.newProject = new ProjectModel();
            base.IsBusy = true;
        }

        protected override void CreateCommands()
        {
            this.SaveCommand = new RelayCommand(async delegate (object o)
            {
                base.IsBusy = true;
                bool flag = await DataController.SaveNewProject(this.newProject);
                base.IsBusy = false;
                if (flag)
                {
                    this.newProject.Reset();
                }
                MessageBox.Show(flag ? "Successfully saved project." : "Could not save project at this time.", flag ? "Success" : "Error", MessageBoxButton.OK, flag ? MessageBoxImage.Asterisk : MessageBoxImage.Exclamation);
                base.IsBusy = false;
            }, (object o) => this.CanSave());
        }

        private bool CanSave()
        {
            return this.newProject.StartDate < this.newProject.EndDate && !string.IsNullOrWhiteSpace(this.newProject.Name);
        }

        public override void Reset()
        {
        }
    }
}
