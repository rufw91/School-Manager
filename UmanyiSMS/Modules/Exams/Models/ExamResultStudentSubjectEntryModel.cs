using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultStudentSubjectEntryModel : ExamResultSubjectEntryModel
    {
        private int studentID;

        private string name;

        public int StudentID
        {
            get
            {
                return this.studentID;
            }
            set
            {
                if (value != this.studentID)
                {
                    this.studentID = value;
                    base.NotifyPropertyChanged("StudentID");
                }
            }
        }

        public string NameOfStudent
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    base.NotifyPropertyChanged("NameOfStudent");
                }
            }
        }

        public ExamResultStudentSubjectEntryModel()
        {
            this.StudentID = 0;
            this.NameOfStudent = "";
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "MaximumScore" || e.PropertyName == "Score")
                {
                    this.CheckErrors();
                }
            };
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            if (base.Score > base.OutOf)
            {
                base.SetErrors("Score", new List<string>
                {
                    string.Concat(new object[]
                    {
                        "Score value [",
                        base.Score,
                        "] is invalid. Should be non negative number greater than zero and less than or equal to [",
                        base.MaximumScore,
                        "]"
                    })
                });
            }
            return base.HasErrors;
        }

        public override void Reset()
        {
            base.Reset();
            this.StudentID = 0;
            this.NameOfStudent = "";
        }
    }
}
