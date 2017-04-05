using System;
using System.Collections.Generic;
using System.ComponentModel;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultSubjectEntryModel : SubjectModel
    {
        private string remarks;

        private decimal score;

        private int examResultID;

        private decimal outOf;

        public int ExamResultID
        {
            get
            {
                return this.examResultID;
            }
            set
            {
                if (value != this.examResultID)
                {
                    this.examResultID = value;
                    base.NotifyPropertyChanged("ExamResultID");
                }
            }
        }

        public decimal Score
        {
            get
            {
                return this.score;
            }
            set
            {
                if (value != this.score)
                {
                    this.score = value;
                    base.NotifyPropertyChanged("Score");
                }
            }
        }

        public decimal OutOf
        {
            get
            {
                return this.outOf;
            }
            set
            {
                if (value != this.outOf)
                {
                    this.outOf = value;
                    base.NotifyPropertyChanged("OutOf");
                }
            }
        }

        public string Remarks
        {
            get
            {
                return this.remarks;
            }
            set
            {
                if (value != this.remarks)
                {
                    this.remarks = value;
                    base.NotifyPropertyChanged("Remarks");
                }
            }
        }

        public ExamResultSubjectEntryModel()
        {
            this.ExamResultID = 0;
            this.Score = 0m;
            this.outOf = 100m;
            this.Remarks = "";
            base.Tutor = "";
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Score" || e.PropertyName == "NameOfSubject" || e.PropertyName == "OutOf")
                {
                    CheckErrors();
                    if (!base.HasErrors)
                    {
                        this.Remarks = Institution.Controller.DataController.GetRemark((100m/OutOf)*this.score,this.Code==102);
                    }
                }
            };
        }

        public ExamResultSubjectEntryModel(SubjectModel sm)
        {
            base.NameOfSubject = sm.NameOfSubject;
            base.SubjectID = sm.SubjectID;
            this.OutOf = sm.MaximumScore;
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Score" || e.PropertyName == "NameOfSubject" || e.PropertyName == "OutOf")
                {
                    CheckErrors();
                    if (!base.HasErrors)
                    {
                        this.Remarks = Institution.Controller.DataController.GetRemark((100m / OutOf) * this.score, this.Code == 102);
                    }
                }
            };
        }

        public ExamResultSubjectEntryModel(SubjectBaseModel sm)
        {
            base.NameOfSubject = sm.NameOfSubject;
            base.SubjectID = sm.SubjectID;
            this.OutOf = 100;
            base.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Score" || e.PropertyName == "NameOfSubject" || e.PropertyName == "OutOf")
                {
                    CheckErrors();
                    if (!base.HasErrors)
                    {
                        this.Remarks = Institution.Controller.DataController.GetRemark((100m / OutOf) * this.score, this.Code == 102);
                    }
                }
            };
        }

        public override bool CheckErrors()
        {
            base.ClearErrors("Score");
            if (this.score > this.outOf)
            {
                base.SetErrors("Score", new List<string>
                        {
                            string.Concat(new object[]
                            {
                                "Value [",
                                this.score,
                                "] should be a non-negative number less than or equal to [",
                                this.outOf,
                                "]"
                            })
                        });
            }

            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }

        public override void Reset()
        {
            base.Reset();
            this.ExamResultID = 0;
            this.Score = 0m;
            this.Remarks = "";
            base.Tutor = "";
        }
    }
}
