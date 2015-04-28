using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper.Models
{
    public class Lesson:NotifiesPropertyChanged
    {
        private string subject;
        private string tutor;
        private TimeSpan endTime;
        private TimeSpan startTime;
        private int subjectIndex;
        public Lesson()
        {
            Subject = "";
            Tutor = "";
            StartTime = DateTime.Now.TimeOfDay;
            StartTime = DateTime.Now.AddMinutes(40).TimeOfDay;
            SubjectIndex = 1;
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (subject != value)
                    subject = value;
                NotifyPropertyChanged("Subject");
            }
        }
        public string Tutor
        {
            get { return tutor; }
            set
            {
                if (tutor != value)
                    tutor = value;
                NotifyPropertyChanged("Tutor");
            }
        }
        public TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                    startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                    endTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }
        
        public TimeSpan Duration { get; set; }

        public int SubjectIndex
        {
            get { return subjectIndex; }
            set
            {
                if (subjectIndex != value)
                    subjectIndex = value;
                NotifyPropertyChanged("SubjectIndex");
            }
        }
    }
}
