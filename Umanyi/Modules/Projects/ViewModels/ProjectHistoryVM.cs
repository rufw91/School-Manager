
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Students.Models;
using UmanyiSMS.Modules.Projects.Controller;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Projects.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ProjectHistoryVM : ViewModelBase
    {
        private ObservableCollection<ProjectBaseModel> allProjects;

        private ObservableCollection<ProjectTaskModel> allTasks;

        private ProjectTaskModel newTask;

        private int selectedProjectID;

        private ProjectTaskModel selectedTask;

        public int SelectedProjectID
        {
            get
            {
                return this.selectedProjectID;
            }
            set
            {
                if (value != this.selectedProjectID)
                {
                    this.selectedProjectID = value;
                    base.NotifyPropertyChanged("SelectedProjectID");
                }
            }
        }

        public ProjectTaskModel SelectedTask
        {
            get
            {
                return this.selectedTask;
            }
            set
            {
                if (value != this.selectedTask)
                {
                    this.selectedTask = value;
                    base.NotifyPropertyChanged("SelectedTask");
                }
            }
        }

        public ProjectTaskModel NewTask
        {
            get
            {
                return this.newTask;
            }
            set
            {
                if (value != this.newTask)
                {
                    this.newTask = value;
                    base.NotifyPropertyChanged("NewTask");
                }
            }
        }

        public ObservableCollection<ProjectBaseModel> AllProjects
        {
            get
            {
                return this.allProjects;
            }
            set
            {
                if (value != this.allProjects)
                {
                    this.allProjects = value;
                    base.NotifyPropertyChanged("AllProjects");
                }
            }
        }

        public ObservableCollection<ProjectTaskModel> AllTasks
        {
            get
            {
                return this.allTasks;
            }
            set
            {
                if (value != this.allTasks)
                {
                    this.allTasks = value;
                    base.NotifyPropertyChanged("AllTasks");
                }
            }
        }

        public System.Windows.Input.ICommand SaveCommand
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand AddCommand
        {
            get;
            private set;
        }

        public System.Windows.Input.ICommand RemoveCommand
        {
            get;
            private set;
        }

        public ProjectHistoryVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override async void InitVars()
        {
            base.Title = "PROJECT TIMELINE";
            this.allTasks = new ObservableCollection<ProjectTaskModel>();
            this.NewTask = new ProjectTaskModel();
            this.AllProjects = await DataController.GetAllProjectsDisplay();
            this.SelectedProjectID = 0;
            base.PropertyChanged += async delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "SelectedProjectID" && this.selectedProjectID != 0)
                {
                    this.AllTasks = await DataController.GetProjectTasksAsync(this.selectedProjectID);
                }
            };
        }

        protected override void CreateCommands()
        {
            this.AddCommand = new RelayCommand(delegate (object o)
            {
                this.allTasks.Add(this.newTask);
                this.NewTask = new ProjectTaskModel();
            }, (object o) => this.CanAdd());
            this.SaveCommand = new RelayCommand(async delegate (object o)
            {
                base.IsBusy = true;
                bool flag = await DataController.SaveNewProjectTimeLineAsync(this.selectedProjectID, this.allTasks);
                base.IsBusy = false;
                if (flag)
                {
                    this.Reset();
                }
                MessageBox.Show(flag ? "Successfully saved project details." : "Could not save project details at this time.", flag ? "Success" : "Error", MessageBoxButton.OK, flag ? MessageBoxImage.Asterisk : MessageBoxImage.Exclamation);
                base.IsBusy = false;
            }, (object o) => this.Cansave());
            this.RemoveCommand = new RelayCommand(delegate (object o)
            {
                this.allTasks.Remove(this.selectedTask);
            }, (object o) => this.CanRemove());
        }

        private bool CanAdd()
        {
            return this.newTask.Allocation > 0m && !string.IsNullOrWhiteSpace(this.newTask.NameOfTask) && this.newTask.StartDate < this.newTask.EndDate;
        }

        private bool CanRemove()
        {
            return this.selectedTask != null;
        }

        private bool Cansave()
        {
            return !base.IsBusy && this.selectedProjectID > 0 && this.allTasks.Count > 0;
        }

        public override void Reset()
        {
            this.NewTask = new ProjectTaskModel();
        }
    }
}
