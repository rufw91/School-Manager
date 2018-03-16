using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Projects.Models
{
    public class ProjectDetailModel : ModelBase
    {
        private int projectID;

        private string name;

        private DateTime startDate;

        private DateTime endDate;

        private decimal allocation;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    base.NotifyPropertyChanged("Name");
                }
            }
        }

        public int ProjectID
        {
            get
            {
                return this.projectID;
            }
            set
            {
                if (value != this.projectID)
                {
                    this.projectID = value;
                    base.NotifyPropertyChanged("ProjectID");
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

        public decimal Allocation
        {
            get
            {
                return this.allocation;
            }
            set
            {
                if (value != this.allocation)
                {
                    this.allocation = value;
                    base.NotifyPropertyChanged("Allocation");
                }
            }
        }

        public override void Reset()
        {
        }
    }
}
