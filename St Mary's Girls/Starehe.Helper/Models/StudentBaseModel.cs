
using System;
using System.Collections.Generic;
namespace Helper.Models
{
    public class StudentBaseModel : ModelBase
    {
        int _studentID;
        string _name;
        public StudentBaseModel()
        {
            StudentID = 0;
            NameOfStudent = "";
        }
        public StudentBaseModel(int studentID, string nameOfStudent)
        {
            StudentID = studentID;
            NameOfStudent = nameOfStudent;
        }
        public int StudentID
        {
            get { return this._studentID; }

            set
            {
                if (value != this._studentID)
                {
                    this._studentID = value;
                    NotifyPropertyChanged("StudentID");
                }
            }
        }
        public string NameOfStudent
        {
            get { return this._name; }

            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    NotifyPropertyChanged("NameOfStudent");
                }
            }
        }

        public override bool CheckErrors()
        {
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Incomplete;
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
            }
            NotifyPropertyChanged("HasErrors");
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Complete;
            return HasErrors;
        }

        public override void Reset()
        {
            StudentID = 0;
            NameOfStudent = "";
        }
    }
}
