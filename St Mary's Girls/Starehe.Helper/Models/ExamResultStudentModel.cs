using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ExamResultStudentModel : ExamResultBaseModel
    {
        private string nameOfStudent;
        private int studentID;
        private decimal total;
        ObservableCollection<ExamResultSubjectEntryModel> entries;

        public ExamResultStudentModel()
        {
            StudentID = 0;
            NameOfStudent = "";
            Total = 0;
            Entries = new ObservableCollection<ExamResultSubjectEntryModel>();
            entries.CollectionChanged += (o, e) =>
                {
                    Total = 0;
                    foreach (var s in entries)
                        Total += s.Score;
                };
        }

        public ObservableCollection<ExamResultSubjectEntryModel> Entries
        {
            get { return entries; }

            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
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
        public decimal Total
        {
            get { return this.total; }

            set
            {
                if (value != this.total)
                {
                    this.total = value;
                    NotifyPropertyChanged("Total");
                }
            }
        }
        public string NameOfStudent
        {
            get { return this.nameOfStudent; }

            set
            {
                if (value != this.nameOfStudent)
                {
                    this.nameOfStudent = value;
                    NotifyPropertyChanged("NameOfStudent");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            StudentID = 0;
            NameOfStudent = "";
            Entries = new ObservableCollection<ExamResultSubjectEntryModel>();
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
            return HasErrors;
        }
    }
}
