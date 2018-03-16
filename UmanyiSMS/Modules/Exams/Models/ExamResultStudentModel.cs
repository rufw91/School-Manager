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
        
        private decimal totalPoints;
        
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

        public decimal TotalPoints
        {
            get
            {
                return this.totalPoints;
            }
            set
            {
                if (value != this.totalPoints)
                {
                    this.totalPoints = value;
                    base.NotifyPropertyChanged("TotalPoints");
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
            this.MeanGrade = "E";
            this.TotalPoints = 0;
        }

        public override void Reset()
        {
            base.Reset();
            this.StudentID = 0;
            this.NameOfStudent = "";
            this.Total = 0m;
            this.MeanGrade = "E";
            this.TotalPoints = 0;
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
                        if (!student.IsActive)
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
