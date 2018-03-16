using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Exams.Sync.Models
{
    public class SyncExamResultModel
    {
        public SyncExamResultModel()
        {
            Entries = new List<SyncExamResultEntryModel>();
        }

        public string NameOfExam { get; set; }

        public string MeanGrade { get; set; }

        public string Points { get; set; }

        public string TotalScore { get; set; }

        public string ClassPosition { get; set; }

        public string OverAllPosition { get; set; }

        public List<SyncExamResultEntryModel> Entries { get; set; }
    }
}
