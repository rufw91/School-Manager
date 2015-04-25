using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ClassLessons : ObservableCollection<Lesson>
    {
        private string nameOfClass;
        private int classID;
        private DayOfWeek day;
        public ClassLessons()
        {
            NameOfClass = "Class";
        }
        public ClassLessons(IEnumerable<Lesson> collection):base(collection)
        {
            NameOfClass = "Class";
        }
        public DayOfWeek Day
        {
            get { return day; }

            set
            {
                if (value != day)
                {
                    day = value;
                    NotifyPropertyChanged("Day");
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

        public string NameOfClass
        {
            get { return this.nameOfClass; }

            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public void AddItems(IEnumerable<Lesson> items)
        {
            foreach (var t in items)
                Add(t);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
