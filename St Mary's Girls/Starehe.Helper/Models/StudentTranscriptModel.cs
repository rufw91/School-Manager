using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentTranscriptModel: StudentExamResultModel
    {
        private int kcpeScore;
        private string responsibilities;
        private string clubsAndSport;
        private string boarding;
        private string classTeacher;
        private string classTeacherComments;
        private string principal;
        private string principalComments;
        private DateTime openingDay;
        private DateTime closingDay;
        private int cat1Score;
        private int cat2Score;
        private int examScore;
        private string term1Pos;
        private string term2Pos;
        private string term3Pos;
        private ObservableCollection<StudentExamResultEntryModel> entries;
        private int studentTranscriptID;
        private DateTime dateSaved;

        public StudentTranscriptModel()
        {
            StudentTranscriptID = 0;
            KCPEScore = 0;
            Responsibilities = "";
            ClubsAndSport = "";
            Boarding = "";
            ClassTeacher = "";
            ClassTeacherComments = "";
            Principal = "";
            PrincipalComments = "";
            OpeningDay = DateTime.Now;
            ClosingDay = DateTime.Now;
            CAT1Score = 0;
            CAT2Score = 0;
            ExamScore = 0;
            Term1Pos = "1/1";
            Term2Pos = "1/1";
            Term3Pos = "1/1";
            Entries = new ObservableCollection<StudentExamResultEntryModel>();
            DateSaved = DateTime.Now;
        }
        public int StudentTranscriptID
        {
            get { return this.studentTranscriptID; }

            set
            {
                if (value != this.studentTranscriptID)
                {
                    this.studentTranscriptID = value;
                    NotifyPropertyChanged("StudentTranscriptID");
                }
            }
        }
       
        public int KCPEScore
        {
            get { return this.kcpeScore; }

            set
            {
                if (value != this.kcpeScore)
                {
                    this.kcpeScore = value;
                    NotifyPropertyChanged("KCPEScore");
                }
            }
        }

        public string Responsibilities
        {
            get { return this.responsibilities; }

            set
            {
                if (value != this.responsibilities)
                {
                    this.responsibilities = value;
                    NotifyPropertyChanged("Responsibilities");
                }
            }
        }

        public string ClubsAndSport
        {
            get { return this.clubsAndSport; }

            set
            {
                if (value != this.clubsAndSport)
                {
                    this.clubsAndSport = value;
                    NotifyPropertyChanged("ClubsAndSport");
                }
            }
        }

        public string Boarding
        {
            get { return this.boarding; }

            set
            {
                if (value != this.boarding)
                {
                    this.boarding = value;
                    NotifyPropertyChanged("Boarding");
                }
            }
        }

        public string ClassTeacher
        {
            get { return this.classTeacher; }

            set
            {
                if (value != this.classTeacher)
                {
                    this.classTeacher = value;
                    NotifyPropertyChanged("ClassTeacher");
                }
            }
        }

        public string ClassTeacherComments
        {
            get { return this.classTeacherComments; }

            set
            {
                if (value != this.classTeacherComments)
                {
                    this.classTeacherComments = value;
                    NotifyPropertyChanged("ClassTeacherComments");
                }
            }
        }

        public string Principal
        {
            get { return this.principal; }

            set
            {
                if (value != this.principal)
                {
                    this.principal = value;
                    NotifyPropertyChanged("Principal");
                }
            }
        }

        public string PrincipalComments
        {
            get { return this.principalComments; }

            set
            {
                if (value != this.principalComments)
                {
                    this.principalComments = value;
                    NotifyPropertyChanged("PrincipalComments");
                }
            }
        }

        public DateTime OpeningDay
        {
            get { return this.openingDay; }

            set
            {
                if (value != this.openingDay)
                {
                    this.openingDay = value;
                    NotifyPropertyChanged("OpeningDay");
                }
            }
        }

        public DateTime ClosingDay
        {
            get { return this.closingDay; }

            set
            {
                if (value != this.closingDay)
                {
                    this.closingDay = value;
                    NotifyPropertyChanged("ClosingDay");
                }
            }
        }

        public int CAT1Score
        {
            get { return this.cat1Score; }

            set
            {
                if (value != this.cat1Score)
                {
                    this.cat1Score = value;
                    NotifyPropertyChanged("CAT1Score");
                }
            }
        }

        public int CAT2Score
        {
            get { return this.cat2Score; }

            set
            {
                if (value != this.cat2Score)
                {
                    this.cat2Score = value;
                    NotifyPropertyChanged("CAT2Score");
                }
            }
        }

        public int ExamScore
        {
            get { return this.examScore; }

            set
            {
                if (value != this.examScore)
                {
                    this.examScore = value;
                    NotifyPropertyChanged("ExamScore");
                }
            }
        }

        public string Term1Pos
        {
            get { return this.term1Pos; }

            set
            {
                if (value != this.term1Pos)
                {
                    this.term1Pos = value;
                    NotifyPropertyChanged("Term1Pos");
                }
            }
        }

        public string Term2Pos
        {
            get { return this.term2Pos; }

            set
            {
                if (value != this.term2Pos)
                {
                    this.term2Pos = value;
                    NotifyPropertyChanged("Term2Pos");
                }
            }
        }

        public string Term3Pos
        {
            get { return this.term3Pos; }

            set
            {
                if (value != this.term3Pos)
                {
                    this.term3Pos = value;
                    NotifyPropertyChanged("Term3Pos");
                }
            }
        }

        public new ObservableCollection<StudentExamResultEntryModel> Entries
        {
            get { return this.entries; }

            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public DateTime DateSaved
        {
            get { return this.dateSaved; }

            set
            {
                if (value != this.dateSaved)
                {
                    this.dateSaved = value;
                    NotifyPropertyChanged("DateSaved");
                }
            }
        }

        public void CopyFrom(StudentTranscriptModel newTranscript)
        {
            base.CopyFrom(newTranscript);
            StudentTranscriptID = newTranscript.StudentTranscriptID;
            KCPEScore = newTranscript.KCPEScore;
            Responsibilities = newTranscript.Responsibilities;
            ClubsAndSport = newTranscript.ClubsAndSport;
            Boarding = newTranscript.Boarding;
            ClassTeacher = newTranscript.ClassTeacher;
            ClassTeacherComments = newTranscript.ClassTeacherComments;
            Principal = newTranscript.Principal;
            PrincipalComments = newTranscript.PrincipalComments;
            OpeningDay = newTranscript.OpeningDay;
            ClosingDay = newTranscript.ClosingDay;
            CAT1Score = newTranscript.CAT1Score;
            CAT2Score = newTranscript.CAT2Score;
            ExamScore = newTranscript.ExamScore;
            Term1Pos = newTranscript.Term1Pos;
            Term2Pos = newTranscript.Term2Pos;
            Term3Pos = newTranscript.Term3Pos;
            Entries = newTranscript.Entries;
        }
        public override void Reset()
        {
            base.Reset();
            StudentTranscriptID = 0;
            KCPEScore = 0;
            Responsibilities = "";
            ClubsAndSport = "";
            Boarding = "";
            ClassTeacher = "";
            ClassTeacherComments = "";
            Principal = "";
            PrincipalComments = "";
            OpeningDay = DateTime.Now;
            ClosingDay = DateTime.Now;
            CAT1Score = 0;
            CAT2Score = 0;
            ExamScore = 0;
            Term1Pos = "1/1";
            Term2Pos = "1/1";
            Term3Pos = "1/1";
            Entries.Clear();

        }

    }
}
