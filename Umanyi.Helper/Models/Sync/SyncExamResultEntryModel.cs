using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models.Sync
{
    public class SyncExamResultEntryModel
    {
        public SyncExamResultEntryModel()
        {

        }

        public string NameOfSubject
        { get; set; }

        public string Score
        { get; set; }

        public string Grade
        { get; set; }

        public string Points
        { get; set; }

        public string Code
        { get; set; }

        public string Remarks
        { get; set; }
    }
}
