using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AcademicYearModel: ModelBase
    {
        private DateTime endDate;
        private DateTime startDate;
        private int noOfTerms;
        private string description;
        private ObservableCollection<TermModel> allTerms;

        public AcademicYearModel()
        {
            AllTerms = new ObservableCollection<TermModel>();
            StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            EndDate = new DateTime(DateTime.Now.Year, 12, 31);
            Description = DateTime.Now.Year.ToString();
            NoOfTerms = 0;
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        public int NoOfTerms
        {
            get { return noOfTerms; }
            set
            {
                if (value != noOfTerms)
                {
                    noOfTerms = value;
                    NotifyPropertyChanged("NoOfTerms");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<TermModel> AllTerms
        {
            get { return allTerms; }
            set
            {
                if (value != allTerms)
                {
                    allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        public override void Reset()
        {
            allTerms.Clear();
            noOfTerms = 0;
            StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            EndDate = new DateTime(DateTime.Now.Year, 12, 31);
            Description = DateTime.Now.Year.ToString();
        }

    }
}
