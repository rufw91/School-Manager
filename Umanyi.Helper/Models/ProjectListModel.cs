using System;

namespace Helper.Models
{
    public class ProjectListModel : ProjectBaseModel
    {
        private decimal budget;

        private decimal curentAllocation;
        private DateTime endDate;
        private DateTime startDate;


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

        public decimal CurrentAllocation
        {
            get
            {
                return this.curentAllocation;
            }
            set
            {
                if (value != this.curentAllocation)
                {
                    this.curentAllocation = value;
                    base.NotifyPropertyChanged("CurrentAllocation");
                }
            }
        }

        public ProjectListModel()
        {
            this.Budget = 0m;
            this.CurrentAllocation = 0m;
        }

        public override void Reset()
        {
            base.Reset();
            this.CurrentAllocation = 0m;
            this.Budget = 0m;
        }
    }
}
