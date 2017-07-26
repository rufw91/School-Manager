using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Projects.Models
{
    public class ProjectTaskModel : ModelBase
    {
        private int taskID;

        private decimal amount;

        private string nameOfTask;

        private DateTime startDate;

        private DateTime endDate;

        public int TaskID
        {
            get
            {
                return this.taskID;
            }
            set
            {
                if (value != this.taskID)
                {
                    this.taskID = value;
                    base.NotifyPropertyChanged("TaskID");
                }
            }
        }

        public string NameOfTask
        {
            get
            {
                return this.nameOfTask;
            }
            set
            {
                if (value != this.nameOfTask)
                {
                    this.nameOfTask = value;
                    base.NotifyPropertyChanged("NameOfTask");
                }
            }
        }

        public decimal Allocation
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (value != this.amount)
                {
                    this.amount = value;
                    base.NotifyPropertyChanged("Allocation");
                }
            }
        }

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

        public ProjectTaskModel()
        {
            this.TaskID = new Random().Next(-2147483648, 2147483647);
            this.NameOfTask = "";
            this.Allocation = 0m;
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.AddDays(1.0);
        }

        public override void Reset()
        {
            this.NameOfTask = "";
            this.Allocation = 0m;
            this.TaskID = new Random().Next(-2147483648, 2147483647);
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.AddDays(1.0);
        }
    }
}
