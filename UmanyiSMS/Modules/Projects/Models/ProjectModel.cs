using System;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Projects.Models
{
    public class ProjectModel : ProjectBaseModel
    {
        private DateTime startDate;

        private DateTime endDate;

        private decimal budget;

        private string description;

        private ObservableCollection<ProjectDetailModel> tasks;

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                if (value != this.startDate)
                {
                    this.startDate = value;
                    base.NotifyPropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                if (value != this.endDate)
                {
                    this.endDate = value;
                    base.NotifyPropertyChanged("EndDate");
                }
            }
        }

        public decimal Budget
        {
            get
            {
                return this.budget;
            }
            set
            {
                if (value != this.budget)
                {
                    this.budget = value;
                    base.NotifyPropertyChanged("Budget");
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    base.NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<ProjectDetailModel> Tasks
        {
            get
            {
                return this.tasks;
            }
            set
            {
                if (value != this.tasks)
                {
                    this.tasks = value;
                    base.NotifyPropertyChanged("Tasks");
                }
            }
        }

        public ProjectModel()
        {
            this.Budget = 0m;
            this.tasks = new ObservableCollection<ProjectDetailModel>();
            this.startDate = DateTime.Now;
            this.endDate = DateTime.Now.AddDays(1.0);
            this.Description = "";
        }

        public override void Reset()
        {
            base.Reset();
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.AddDays(1.0);
            this.Budget = 0m;
            this.Description = "";
            this.tasks.Clear();
        }
    }
}
