using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TimeTableSettingsModel
    {
        public TimeTableSettingsModel()
        {
            TimeTableSettingsID = 0;
            NoOfLessons = 10;
            LessonDuration = new TimeSpan(0,40,0);
            LessonsStartTime = new TimeSpan(8,0,0);
            BreakIndices = new List<int>(){2,4,7};
            BreakDurations = new List<TimeSpan>(){new TimeSpan(0,10,0),new TimeSpan(0,30,0),new TimeSpan(0,10,0)};
        }

        public int TimeTableSettingsID { get; set; }

        public int NoOfLessons { get; set; }

        public TimeSpan LessonDuration { get; set; }

        public TimeSpan LessonsStartTime { get; set; }

        public List<int> BreakIndices { get; set; }

        public List<TimeSpan> BreakDurations { get; set; }

    }
}
