using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ExamRegistrationStudentModel : StudentSelectModel
    {
        private int examID;

        public ExamRegistrationStudentModel()
        {
            ExamID = 0;
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            base.CheckErrors();
            if (ExamID > 0)
            {
                bool isRegistered = DataAccess.CheckIfRegisteredForExam(StudentID, ExamID).Result;
                if (isRegistered)
                {
                    SetErrors("StudentID", new List<string>
                    {
                        "Student already registered."
                    });
                }
            }

            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }

        public int ExamID
        {
            get { return this.examID; }

            set
            {
                if (value != this.examID)
                {
                    this.examID = value;
                    NotifyPropertyChanged("ExamID");
                }
            }
        }


        public override void Reset()
        {
            base.Reset();
            ExamID = 0;
        }
    }
}
