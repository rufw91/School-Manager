
using System;
namespace Helper.Models
{
    public class EventModel:ModelBase
    {
        int eventID;
        string name;
        DateTime startDatetime;
        DateTime endDatetime;
        DateTime startDate;
        DateTime endDate;
        TimeSpan startTime;
        TimeSpan endTime;
        string location;
        string message;
        string subject;
        public EventModel()
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "StartDate") || (e.PropertyName == "StartTime"))
                    StartDateTime = StartDate + StartTime;
                if ((e.PropertyName == "EndDate") || (e.PropertyName == "EndTime"))
                    EndDateTime = EndDate + EndTime;
            };
            EventID = 0;
            Name = "";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            StartTime = new TimeSpan(0, 0, 0);
            EndTime = new TimeSpan(23, 0, 0);
            Location = "";
            Subject = "";
            Message = "";            
        }
        public int EventID
        {
            get { return eventID; }
            set
            {
                if (eventID != value)
                {
                    eventID = value;
                    NotifyPropertyChanged("EventID");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public DateTime StartDateTime
        {
            get { return startDatetime; }
            set
            {
                if (startDatetime != value)
                {
                    startDatetime = value;
                    NotifyPropertyChanged("StartDateTime");
                }
            }
        }

        public DateTime EndDateTime
        {
            get { return endDatetime; }
            set
            {
                if (endDatetime != value)
                {
                    endDatetime = value;
                    NotifyPropertyChanged("EndDateTime");
                }
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
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
                if (endDate != value)
                {
                    endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        public TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    NotifyPropertyChanged("EndTime");
                }
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    NotifyPropertyChanged("Subject");
                }
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public override void Reset()
        {
            EventID = 0;
            Name = "";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            StartTime = new TimeSpan(0, 0, 0);
            EndTime = new TimeSpan(23, 0, 0);
            Location = "";
            Subject = "";
            Message = "";
        }
    }
}
