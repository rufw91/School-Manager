using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TimetableColumnHeaderModel:NotifiesPropertyChanged
    {
        private string duration;
        private string title;

        public TimetableColumnHeaderModel()
        {
            Title = "";
            Duration = "";
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    NotifyPropertyChanged("Duration");
                }
            }
        }
    }
}
