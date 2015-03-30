using Helper;
using Helper.Models;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class AcademicReportVM : ReportViewModel
    {
        private int? studentID;
        private int? classID;
        private int? examID;
        private string grade;
        private decimal? score;
        private FixedDocument document;
        public AcademicReportVM():base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Columns.Add(new ColumnModel(true,"Student ID",0.8));
            Columns.Add(new ColumnModel(true, "Name of Student", 1));
            Columns.Add(new ColumnModel(true, "Class", 1));
            Columns.Add(new ColumnModel(true, "Exam", 1));
            Columns.Add(new ColumnModel(true, "Grade", .3));
            Columns.Add(new ColumnModel(true, "Score", .5));
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                AcademicReportModel arm = await DataAccess.GetAcademicReport(studentID,classID,examID,grade,score);
                Document = DocumentHelper.GenerateDocument(arm);

            },o=>!IsBusy);
        }

        public int? StudentID
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

        public int? ClassID
        {
            get { return this.classID; }

            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }

        public int? ExamID
        {
            get { return this.examID; }

            set
            {
                if (value != this.examID)
                {
                    this.examID = value;
                    NotifyPropertyChanged("ExamID");
                }
            }
        }

        public string Grade
        {
            get { return this.grade; }

            set
            {
                if (value != this.grade)
                {
                    this.grade = value;
                    NotifyPropertyChanged("Grade");
                }
            }
        }

        public decimal? Score
        {
            get { return this.score; }

            set
            {
                if (value != this.score)
                {
                    this.score = value;
                    NotifyPropertyChanged("Score");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }

        public FixedDocument Document
        {
            get { return this.document; }

            set
            {
                if (value != this.document)
                {
                    this.document = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
    }
}
