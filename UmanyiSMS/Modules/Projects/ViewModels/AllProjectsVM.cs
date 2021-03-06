﻿

using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;
namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class AllProjectsVM : ViewModelBase
    {
        private ObservableCollection<ProjectListModel> projects;

        public ObservableCollection<ProjectListModel> AllProjects
        {
            get
            {
                return this.projects;
            }
            private set
            {
                if (value != this.projects)
                {
                    this.projects = value;
                    base.NotifyPropertyChanged("AllProjects");
                }
            }
        }

        public AllProjectsVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override async void InitVars()
        {
            base.Title = "ALL PROJECTS";
            base.IsBusy = true;
            this.AllProjects =  await DataController.GetAllProjects();
            base.IsBusy = false;
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
