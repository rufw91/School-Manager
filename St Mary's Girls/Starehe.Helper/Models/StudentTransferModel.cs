using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentTransferModel: StudentBaseModel
    {
        private DateTime dateTransferred;
        public StudentTransferModel()
        {
            DateTransferred = DateTime.Now;
        }

        public DateTime DateTransferred
        {
            get { return this.dateTransferred; }

            set
            {
                if (value != this.dateTransferred)
                {
                    this.dateTransferred = value;
                    NotifyPropertyChanged("DateTransferred");
                }
            }
        }

        public override bool CheckErrors()
        {            
            try
            {
                ClearAllErrors();
                if (StudentID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student does not exist.");
                    SetErrors("StudentID", errors);
                }
                else
                {
                    StudentModel student = DataAccess.GetStudent(StudentID);
                    if (student.StudentID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student does not exist.");
                        SetErrors("StudentID", errors);
                    }
                    else
                    {
                        ClearErrors("StudentID");
                        this.NameOfStudent = student.NameOfStudent;
                    }
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            return HasErrors;    
        }
        
        public override void Reset()
        {
            base.Reset();
            DateTransferred = DateTime.Now;
        }
    }
}
