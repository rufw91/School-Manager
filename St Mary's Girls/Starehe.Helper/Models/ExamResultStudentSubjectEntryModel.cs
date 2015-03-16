using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ExamResultStudentSubjectEntryModel : ExamResultSubjectEntryModel
    {
        int studentID;
        string name;
        public ExamResultStudentSubjectEntryModel()
        {            
            StudentID = 0;
            NameOfStudent = "";
        }

        public int StudentID
        {
            get { return this.studentID; }

            set
            {
                if (value != this.studentID)
                {
                    this.studentID = value;
                    NotifyPropertyChanged("StudentID");
                }
            }
        }
        public string NameOfStudent
        {
            get { return this.name; }

            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("NameOfStudent");
                }
            }
        }
        public override void Reset()
        {
            base.Reset();
            StudentID = 0;
            NameOfStudent = "";
            
        }
    }
}
