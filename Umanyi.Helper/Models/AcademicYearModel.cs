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
        private int year;
        private int noOfTerms;
        private string description;
        private ObservableCollection<TermModel> allTerms;

        public AcademicYearModel()
        {
            AllTerms = new ObservableCollection<TermModel>();
            Year = DateTime.Now.Year;
            Description = DateTime.Now.Year.ToString();
            NoOfTerms = 0;
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value != year)
                {
                    year = value;
                    NotifyPropertyChanged("Year");
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
            Year = DateTime.Now.Year;
            Description = DateTime.Now.Year.ToString();
        }

    }
}
