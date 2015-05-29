using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class SubjectModel : SubjectBaseModel
    {
        decimal maximumScore;
        private string tutor;
        private int code;
        private bool isOptional;
        private static ObservableCollection<SubjectModel> allSubjects;

        static SubjectModel()
        {
            allSubjects = new ObservableCollection<SubjectModel>();
            allSubjects.Add(new SubjectModel() { NameOfSubject = "ENGLISH", Code = 101 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "KISWAHILI", Code = 102 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "MATHEMATICS", Code = 121 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "MATHEMATICS B", Code = 122 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "BIOLOGY", Code = 231 });            
            allSubjects.Add(new SubjectModel() { NameOfSubject = "PHYSICS", Code = 232 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "CHEMISTRY", Code = 233 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "BIOLOGY FOR THE BLIND", Code = 236 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "GENERAL SCIENCE", Code = 237 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "HISTORY", Code = 311 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "GEOGRAPHY", Code = 312 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "CRE", Code = 313 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "IRE", Code = 314 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "HRE", Code = 315 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "HOME SCIENCE", Code = 441 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "ART AND DESIGN", Code = 442 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "AGRICULTURE", Code = 443 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "WOOD WORK", Code = 444 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "METAL WORK", Code = 445 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "BUILDING CONSTRUCTION", Code = 446 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "POWER MECHANICS", Code = 447 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "ELECTRICITY", Code = 448 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "DRAWING AND DESIGN", Code = 449 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "ELECTRICITY", Code = 450 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "COMPUTER ST", Code = 451 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "FRENCH", Code = 501 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "GERMAN", Code = 502 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "ARABIC", Code = 503 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "SIGN LANGUAGE", Code = 504 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "MUSIC", Code = 511 });
            allSubjects.Add(new SubjectModel() { NameOfSubject = "BUSINESS ST", Code = 565 });
        }

        public SubjectModel()
        {
            MaximumScore = 100;
            Tutor = "";
            Code = 0;
            IsOptional = false;
        }

        public SubjectModel(int subjectID, string nameofsubject, decimal newMaximumScore,bool isOptional)
            : base(subjectID, nameofsubject)
        {
            MaximumScore = newMaximumScore;
            IsOptional = isOptional;
        }
        
        public static ObservableCollection<SubjectModel> AllSubjects
        { get { return allSubjects; } }

        public decimal MaximumScore
        {
            get { return this.maximumScore; }

            set
            {
                if (value != this.maximumScore)
                {
                    this.maximumScore = value;
                    NotifyPropertyChanged("MaximumScore");
                }
            }
        }

        public string Tutor
        {
            get { return this.tutor; }

            set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
                }
            }
        }

        public int Code
        {
            get { return this.code; }

            set
            {
                if (value != this.code)
                {
                    this.code = value;
                    NotifyPropertyChanged("Code");
                }
            }
        }

        public bool IsOptional
        {
            get { return this.isOptional; }

            set
            {
                if (value != this.isOptional)
                {
                    this.isOptional = value;
                    NotifyPropertyChanged("IsOptional");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaximumScore = 0;
            Tutor = "";
            Code = 0;
            IsOptional = false;
        }
    }
}
