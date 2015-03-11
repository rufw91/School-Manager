using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentSelectModel:StudentBaseModel
    {
        public StudentSelectModel()
        {
            CheckErrors();
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
                    NameOfStudent = "";
                }
                else
                {
                    StudentModel student = DataAccess.GetStudent(StudentID);
                    if (student.StudentID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student does not exist.");
                        SetErrors("StudentID", errors);
                        NameOfStudent = "";
                    }
                    else
                    {
                        ClearErrors("StudentID");
                        this.StudentID = student.StudentID;
                        this.NameOfStudent = student.NameOfStudent;
                    }
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
                NameOfStudent = "";
            }
            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }
    }
}
