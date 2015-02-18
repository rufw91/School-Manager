using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TimetableClassEntryModel: SubjectBaseModel
    {
        string day;
        string tutor;
        string period;
        TimeSpan startTime;
        TimeSpan endTime;
        public TimetableClassEntryModel()
        {
            Day = "";
            Tutor = "";
            Period = "";
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.TimeOfDay;
            PropertyChanged += (o, e) =>
                {
                    if ((e.PropertyName == "StartTime") || (e.PropertyName == "EndTime"))
                        Period = startTime.ToString(@"hh\:mm\:ss") + " - " + endTime.ToString(@"hh\:mm\:ss");
                };
        }

        public TimetableClassEntryModel(SubjectBaseModel subject)
            :base(subject.SubjectID,subject.NameOfSubject)
        {
            Day = "";
            Tutor = "";
            Period = "";
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.TimeOfDay;
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "StartTime") || (e.PropertyName == "EndTime"))
                    Period = startTime.ToString() + " - " + endTime.ToString();
            };
        }

        public string Day
        {
            get { return this.day; }

            set
            {
                if (value != this.day)
                {
                    this.day = value;
                    NotifyPropertyChanged("Day");
                }
            }
        }

        public string Tutor
        {
            get { return this.tutor; }

            set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
                }
            }
        }

        public string Period
        {
            get { return this.period; }

            private set
            {
                if (value != this.period)
                {
                    this.period = value;
                    NotifyPropertyChanged("Period");
                }
            }
        }

        public TimeSpan StartTime
        {
            get { return this.startTime; }

            set
            {
                if (value != this.startTime)
                {
                    this.startTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        public TimeSpan EndTime
        {
            get { return this.endTime; }

            set
            {
                if (value != this.endTime)
                {
                    this.endTime = value;
                    NotifyPropertyChanged("EndTime");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            Day = "";
            Tutor = "";
            Period = "";
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.TimeOfDay;
        }
    }
}
