using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultStudentModel : ExamResultBaseModel
    {
        private string nameOfStudent;

        private string meanGrade;

        private int studentID;

        private decimal total;

        private ObservableCollection<ExamResultSubjectEntryModel> entries;

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    base.NotifyPropertyChanged("IsActive");
                }
            }
        }

        public ObservableCollection<ExamResultSubjectEntryModel> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

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

        public decimal Total
        {
            get
            {
                return this.total;
            }
            set
            {
                if (value != this.total)
                {
                    this.total = value;
                    base.NotifyPropertyChanged("Total");
                }
            }
        }

        public string NameOfStudent
        {
            get
            {
                return this.nameOfStudent;
            }
            set
            {
                if (value != this.nameOfStudent)
                {
                    this.nameOfStudent = value;
                    base.NotifyPropertyChanged("NameOfStudent");
                }
            }
        }

        public string MeanGrade
        {
            get
            {
                return this.meanGrade;
            }
            set
            {
                if (value != this.meanGrade)
                {
                    this.meanGrade = value;
                    base.NotifyPropertyChanged("MeanGrade");
                }
            }
        }

        public ExamResultStudentModel()
        {
            this.StudentID = 0;
            this.NameOfStudent = "";
            this.Total = 0m;
            this.CheckErrors();
            this.Entries = new ObservableCollection<ExamResultSubjectEntryModel>();
            this.entries.CollectionChanged += delegate (object o, NotifyCollectionChangedEventArgs e)
            {
                this.Total = 0m;
                this.MeanGrade = "";
                int num = 0;
                foreach (ExamResultSubjectEntryModel current in this.entries)
                {
                    this.Total += current.Score;
                    num += Institution.Controller.DataController.CalculatePoints(Institution.Controller.DataController.CalculateGrade(DataController.ConvertScoreToOutOf(current.Score, current.OutOf, 100m)));
                    this.MeanGrade = Institution.Controller.DataController.CalculateGrade(decimal.Parse((this.Total / this.entries.Count).ToString("N2")));
                }
            };
            this.IsActive = true;
        }

        public override void Reset()
        {
            base.Reset();
            this.StudentID = 0;
            this.NameOfStudent = "";
            this.Entries = new ObservableCollection<ExamResultSubjectEntryModel>();
        }

        public override bool CheckErrors()
        {
            try
            {
                base.ClearAllErrors();
                if (this.StudentID == 0)
                {
                    base.SetErrors("StudentID", new List<string>
                    {
                        "Student does not exist."
                    });
                }
                else
                {
                    StudentModel student = Students.Controller.DataController.GetStudent(this.StudentID);
                    if (student.StudentID == 0)
                    {
                        base.SetErrors("StudentID", new List<string>
                        {
                            "Student does not exist."
                        });
                    }
                    else
                    {
                        base.ClearErrors("StudentID");
                        this.StudentID = student.StudentID;
                        this.NameOfStudent = student.NameOfStudent;
                        this.IsActive = student.IsActive;
                        if (!this.isActive)
                        {
                            base.SetErrors("StudentID", new List<string>
                            {
                                "Student is not active."
                            });
                        }
                        foreach (var s in entries)
                            s.CheckErrors();
                    }
                }
            }
            catch (Exception ex)
            {
                base.SetErrors("", new List<string>
                {
                    ex.Message
                });
            }
            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }
    }
}
