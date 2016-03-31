using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Helper.Models
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
                    base.ClearAllErrors();
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
                    if (!base.HasErrors)
                    {
                        this.Remarks = this.GetRemark(this.score);
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
                    if (!base.HasErrors)
                    {
                        this.Remarks = this.GetRemark(this.score);
                    }
                }
            };
        }

        private string GetRemark(decimal score)
        {
            int num = DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(score, this.outOf, 100m)));
            string result;
            if (base.NameOfSubject.ToUpper().Trim() != "KISWAHILI")
            {
                switch (num)
                {
                    case 1:
                        result = "WAKE UP";
                        return result;
                    case 2:
                        result = "VERY POOR";
                        return result;
                    case 3:
                        result = "POOR";
                        return result;
                    case 4:
                        result = "BELOW AVERAGE";
                        return result;
                    case 5:
                        result = "FAIR";
                        return result;
                    case 6:
                        result = "AVERAGE";
                        return result;
                    case 7:
                        result = "AVERAGE";
                        return result;
                    case 8:
                        result = "ABOVE AVERAGE";
                        return result;
                    case 9:
                        result = "GOOD";
                        return result;
                    case 10:
                        result = "VERY GOOD";
                        return result;
                    case 11:
                        result = "VERY GOOD";
                        return result;
                    case 12:
                        result = "EXCELLENT";
                        return result;
                }
            }
            else
            {
                switch (num)
                {
                    case 1:
                        result = "ZINDUKA";
                        return result;
                    case 2:
                        result = "PUNGUZA MZAHA";
                        return result;
                    case 3:
                        result = "AMKA";
                        return result;
                    case 4:
                        result = "TIA BIDII";
                        return result;
                    case 5:
                        result = "TIA BIDII";
                        return result;
                    case 6:
                        result = "CHINI YA WASTANI";
                        return result;
                    case 7:
                        result = "WASTANI";
                        return result;
                    case 8:
                        result = "HEKO";
                        return result;
                    case 9:
                        result = "VIZURI";
                        return result;
                    case 10:
                        result = "VIZURI SANA";
                        return result;
                    case 11:
                        result = "PONGEZI";
                        return result;
                    case 12:
                        result = "HONGERA";
                        return result;
                }
            }
            result = "";
            return result;
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
