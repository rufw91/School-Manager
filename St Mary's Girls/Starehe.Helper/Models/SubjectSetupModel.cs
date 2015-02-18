using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class SubjectsSetupModel : ModelBase
    {
        int subjectSetupID;
        int classID;
        bool isActive;
        DateTime startDate;
        DateTime? endDate = null;
        ObservableCollection<SubjectsSetupEntryModel> entries;
        public SubjectsSetupModel()
        {
            SubjectSetupID = 0;
            ClassID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries = new ObservableCollection<SubjectsSetupEntryModel>();
        }

        public override void Reset()
        {
            SubjectSetupID = 0;
            ClassID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries = new ObservableCollection<SubjectsSetupEntryModel>();
        }

        public int SubjectSetupID
        {
            get { return subjectSetupID; }
            set
            {
                if (value != subjectSetupID)
                {
                    subjectSetupID = value;
                    NotifyPropertyChanged("SubjectSetupID");
                }
            }
        }

        public int ClassID
        {
            get { return classID; }
            set
            {
                if (value != classID)
                {
                    classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
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

        public DateTime? EndDate
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

        public ObservableCollection<SubjectsSetupEntryModel> Entries
        {
            get { return entries; }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

    }

    public class SubjectsSetupEntryModel : SubjectModel
    {
        int subjectSetupID;
        public SubjectsSetupEntryModel()
        {
            SubjectSetupID = 0;
        }

        public SubjectsSetupEntryModel(SubjectModel subjectModel)
        {
            SubjectSetupID = 0;
            this.MaximumScore = subjectModel.MaximumScore;
            this.NameOfSubject = subjectModel.NameOfSubject;
            this.SubjectID = subjectModel.SubjectID;
        }

        public int SubjectSetupID
        {
            get { return subjectSetupID; }
            set
            {
                if (value != subjectSetupID)
                {
                    subjectSetupID = value;
                    NotifyPropertyChanged("SubjectSetupID");
                }
            }
        }

        public override void Reset()
        {
            SubjectSetupID = 0;
            base.Reset();
        }

        
    }
}
