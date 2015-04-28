using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using Helper.Models;
using System.Threading.Tasks;

namespace Helper.Controls
{
    [TemplatePart(Name = "PART_ColumnHeaderPresenter", Type = typeof(TimeTableColumnHeadersPresenter))]
    public class TimeTable : ItemsControl
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(ObservableCollection<string>), typeof(TimeTable), new PropertyMetadata(new ObservableCollection<string>()));
        public static readonly DependencyProperty NoOfLessonsPerDayProperty = DependencyProperty.Register(
            "NoOfLessonsPerDay", typeof(int), typeof(TimeTable), new PropertyMetadata(10));
        public static readonly DependencyProperty LessonDurationProperty = DependencyProperty.Register(
            "LessonDuration", typeof(TimeSpan), typeof(TimeTable), new PropertyMetadata(new TimeSpan(0, 40, 0)));
        public static readonly DependencyProperty NoOfBreaksPerDayProperty = DependencyProperty.Register(
            "NoOfBreaksPerDay", typeof(int), typeof(TimeTable), new PropertyMetadata(3));
        public static readonly DependencyProperty LessonStartTimeProperty = DependencyProperty.Register(
            "LessonStartTime", typeof(TimeSpan), typeof(TimeTable), new PropertyMetadata(new TimeSpan(8,0,0)));
        public static readonly DependencyProperty BreaksDurationsProperty = DependencyProperty.Register(
            "BreaksDurations", typeof(List<TimeSpan>), typeof(TimeTable), new PropertyMetadata(new List<TimeSpan>()
            {
                new TimeSpan(0, 10, 0),new TimeSpan(0, 30, 0),new TimeSpan(1, 10, 0)
            }));
        public static readonly DependencyProperty BreakIndicesProperty = DependencyProperty.Register(
            "BreakIndices", typeof(List<int>), typeof(TimeTable), new PropertyMetadata(new List<int>() { 2, 4, 7 }));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(TimeTable), new PropertyMetadata(false));

        TimeTableGroupedRowsPresenter monday;
        TimeTableGroupedRowsPresenter tuesday;
        TimeTableGroupedRowsPresenter wednesday;
        TimeTableGroupedRowsPresenter thursday;
        TimeTableGroupedRowsPresenter friday;
        TimeTableGroupedRowsPresenter saturday;
        bool hasTemplate;
        public TimeTable()
        {
        }

        private void RefreshItemsSource()
        {
            Columns = GetColumns();
            if ((ItemsSource == null) || ((ItemsSource as TimeTableModel).Count == 0))
            {
                monday.ItemsSource = null;
                tuesday.ItemsSource = null;
                wednesday.ItemsSource = null;
                thursday.ItemsSource = null;
                friday.ItemsSource = null;
                saturday.ItemsSource = null;
            }
            
            else if (hasTemplate)
            {
                if (monday != null)
                    monday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Monday));
                if (tuesday != null)
                    tuesday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Tuesday));
                if (wednesday != null)
                    wednesday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Wednesday));
                if (thursday != null)
                    thursday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Thursday));
                if (friday != null)
                    friday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Friday));
                if (saturday != null)
                    saturday.ItemsSource = InsertBreaks((ItemsSource as TimeTableModel).Where(o => o.Day == DayOfWeek.Saturday));
            }
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (ItemsSource == null)
                return;
            if (ItemsSource.GetType() != typeof(TimeTableModel))
                throw new InvalidOperationException("ItemSource only accepts values of type: [TimeTableModel]");
            RefreshSettings();
            string msg = TestItemsSource();
            if (!string.IsNullOrWhiteSpace(msg))
                throw new InvalidOperationException(msg);
            RefreshSettings();
            RefreshItemsSource();            
            InvalidateVisual();
        }

        private void RefreshSettings()
        {
            this.NoOfLessonsPerDay = (ItemsSource as TimeTableModel).Settings.NoOfLessons;
            this.LessonDuration = (ItemsSource as TimeTableModel).Settings.LessonDuration;
            this.LessonStartTime = (ItemsSource as TimeTableModel).Settings.LessonsStartTime;
            BreaksDurations = (ItemsSource as TimeTableModel).Settings.BreakDurations;
            BreakIndices = (ItemsSource as TimeTableModel).Settings.BreakIndices;
            NoOfBreaksPerDay = (ItemsSource as TimeTableModel).Settings.BreakIndices.Count;
        }

        string TestItemsSource()
        {
            string temp = "";
            foreach (ClassLessons cls in ItemsSource)
            {
                if (cls.Count != NoOfLessonsPerDay)
                    temp += "- No. of Lessons set for Class: " + cls.NameOfClass + " on " + cls.Day + ", " + cls.Count +
                        " , is not equal to the default no. of classes per day " + NoOfLessonsPerDay + "\r\n";
            }
            return temp;
        }

        private ObservableCollection<ClassLessons> InsertBreaks(IEnumerable<ClassLessons> lessons)
        {
            ObservableCollection<ClassLessons> temp = new ObservableCollection<ClassLessons>();
            ClassLessons cls;
            for (int j = 0; j < lessons.Count(); j++)
            {
                cls = new ClassLessons();
                cls.NameOfClass = lessons.ElementAt(j).NameOfClass;
                cls.Day = lessons.ElementAt(j).Day;
                cls.ClassID = lessons.ElementAt(j).ClassID;
                for (int i = 0; i < NoOfLessonsPerDay; i++)
                {
                    if (BreakIndices.Contains(i))
                        cls.Add(new Lesson() { Subject = "Break" });
                    cls.Add(lessons.ElementAt(j)[i]);
                }
                temp.Add(cls);
            }
            return temp;
        }
        private ObservableCollection<string> GetColumns()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>();
            for (int i = 0; i < NoOfLessonsPerDay; i++)
            {
                if (BreakIndices.Contains(i))
                    temp.Add("Break");
                temp.Add("Lesson " + (i + 1));
            }
            return temp;
        }

        public override void OnApplyTemplate()
        {
            hasTemplate = true;
            base.OnApplyTemplate();
            Columns=GetColumns();
            TimeTableColumnHeadersPresenter ch = GetTemplateChild("PART_ColumnHeaderPresenter") as TimeTableColumnHeadersPresenter;
            ch.ItemsSource = Columns;

            monday = GetTemplateChild("PART_MondayRowsPresenter") as TimeTableGroupedRowsPresenter;
            tuesday = GetTemplateChild("PART_TuesdayRowsPresenter") as TimeTableGroupedRowsPresenter;
            wednesday = GetTemplateChild("PART_WednesdayRowsPresenter") as TimeTableGroupedRowsPresenter;
            thursday = GetTemplateChild("PART_ThursdayRowsPresenter") as TimeTableGroupedRowsPresenter;
            friday = GetTemplateChild("PART_FridayRowsPresenter") as TimeTableGroupedRowsPresenter;
            saturday = GetTemplateChild("PART_SaturdayRowsPresenter") as TimeTableGroupedRowsPresenter;

            RefreshItemsSource();

            if (ch != null)
                ch.Tag = this;
        }

        public ObservableCollection<string> Columns
        {
            get
            {
                return (ObservableCollection<string>)GetValue(ColumnsProperty);
            }

            set
            {
                SetValue(ColumnsProperty, value);
            }
        }

        public int NoOfLessonsPerDay
        {
            get
            {
                return (int)GetValue(NoOfLessonsPerDayProperty);
            }

            set
            {
                SetValue(NoOfLessonsPerDayProperty, value);
            }
        }

        public int NoOfBreaksPerDay
        {
            get
            {
                return (int)GetValue(NoOfBreaksPerDayProperty);
            }

            set
            {
                SetValue(NoOfBreaksPerDayProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
        public TimeSpan LessonStartTime
        {
            get
            {
                return (TimeSpan)GetValue(LessonStartTimeProperty);
            }

            set
            {
                SetValue(LessonStartTimeProperty, value);
            }
        }
        public List<TimeSpan> BreaksDurations
        {
            get
            {
                return (List<TimeSpan>)GetValue(BreaksDurationsProperty);
            }

            set
            {
                SetValue(BreaksDurationsProperty, value);
            }
        }
        public TimeSpan LessonDuration
        {
            get
            {
                return (TimeSpan)GetValue(LessonDurationProperty);
            }

            set
            {
                SetValue(LessonDurationProperty, value);
            }
        }
        public List<int> BreakIndices
        {
            get
            {
                return (List<int>)GetValue(BreakIndicesProperty);
            }

            set
            {
                SetValue(BreakIndicesProperty, value);
            }
        }
    }
}
