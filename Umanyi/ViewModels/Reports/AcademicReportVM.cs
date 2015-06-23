using Helper;
using Helper.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class AcademicReportVM : ViewModelBase
    {
        private int studentID;
        private int classID;
        private ExamModel selectedExam;
        private string grade;
        private decimal? score;
        private FixedDocument document;
        private ObservableCollection<ColumnModel> columns;
        private int selectedComparisonValue1;
        private int selectedComparisonValue2;
        private ObservableCollection<ExamModel> allExams;
        decimal outOf = 0;
        public AcademicReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            SelectedComparisonValue1 = 0;
            SelectedComparisonValue2 = 0;
            AllGrades = new ObservableCollection<string>()
            {
                "A","A-","B+","B","B-","C+","C","C-","D+","D","D-","E"
            };
            AllClasses = new ObservableCollection<ClassModel>();
            AllExams = new ObservableCollection<ExamModel>();
            Columns = new ObservableCollection<ColumnModel>(){  
                new ColumnModel(true, "s.StudentID", "Student ID", 0.8),
           new ColumnModel(true, "s.NameOfStudent", "Name of Student", 1),
           new ColumnModel(true, "c.NameOfClass", "Class", 1),
            new ColumnModel(true, "e.NameOfExam","Exam", 1),
            new ColumnModel(true, "dbo.GetGrade(ISNUll(AVG(CONVERT(decimal,erd.Score*100/e.OutOf)),0)) MeanGrade", "Grade (From Mean Score)", .3),
            new ColumnModel(true, "ISNULL(SUM(CONVERT(decimal,erd.Score*"+outOf+"/e.OutOf)),0) TotalScore","Total Score", .5)
            };
            PropertyChanged += async(o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        ClassID = studentID > 0 ? 0 : classID;
                        if (studentID == 0)
                            AllExams.Clear();
                        else
                        {
                            var s = await DataAccess.GetClassIDFromStudentID(studentID);
                            AllExams = await DataAccess.GetExamsByClass(s);
                        }
                    }
                    if (e.PropertyName == "ClassID")
                    {
                        StudentID = classID > 0 ? 0 : studentID;
                        if (classID == 0)
                            AllExams.Clear();
                        else
                            AllExams = await DataAccess.GetExamsByClass(classID);
                    }

                    if ((e.PropertyName == "SelectedComparisonValue1")&&(selectedComparisonValue1 ==0))
                        Grade = null;
                    if ((e.PropertyName == "SelectedComparisonValue2") && (selectedComparisonValue2 == 0))
                        Score = null;
                    if (e.PropertyName == "SelectedExam"&&selectedExam !=null&&selectedExam.ExamID>0)                   
                        OutOf = selectedExam.OutOf;
                    if (e.PropertyName == "OutOf")
                        Columns[5].Name = "ISNULL(SUM(CONVERT(decimal,erd.Score*" + outOf + "/e.OutOf)),0) TotalScore";

                };
            var f = await DataAccess.GetAllClassesAsync();
            AllClasses.Add(new ClassModel() { NameOfClass = "None", ClassID = 0 });
            foreach (ClassModel cs in f)
                AllClasses.Add(cs);
            NotifyPropertyChanged("AllClasses");
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Academic Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Academic Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                var xdc = DocumentHelper.GenerateDocument(rt);
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(xdc);
            }, o => CanRefresh());
        }

        private bool CanRefresh()
        {
            return !IsBusy && selectedExam!=null&&selectedExam.ExamID> 0 && (studentID > 0 || classID > 0) && ((selectedComparisonValue1 > 0) ? !string.IsNullOrWhiteSpace(grade) : true)
                && ((selectedComparisonValue2 > 0) ? score.HasValue : true);
        }

        private Task<DataTable> GetEntries()
        {
            return Task.Run<DataTable>(() =>
            {
                string selectStr = "SELECT * FROM (SELECT ";
                var t = columns.Where(ox => ox.IsSelected == true);
                foreach (var c in t)
                    selectStr += c.Name + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += " FROM [Institution].[Student] s INNER JOIN  [Institution].[ExamResultHeader] erh ON(s.StudentID =erh.StudentID)"+
                    "INNER JOIN [Institution].[ExamResultDetail] erd ON(erh.ExamResultID = erd.ExamResultID) "+
                    "LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[ExamHeader] e ON(erh.ExamID=e.ExamID) WHERE erh.IsActive = 1";
                if (classID > 0)
                    selectStr += " AND s.ClassID=" + classID;
                else if (studentID>0)
                    selectStr += " AND s.StudentID=" + studentID;
                selectStr += " AND erh.ExamID=" + selectedExam.ExamID;
                selectStr += " GROUP BY s.StudentID,s.NameOfStudent,c.NameOfClass,e.NameOfExam) x";
                if (selectedComparisonValue1>0)
                {
                    switch (selectedComparisonValue1)
                    {
                        case 1: selectStr += " WHERE x.MeanGrade =" + grade; break;
                        case 2: selectStr += " WHERE x.MeanGrade IN(" + GetGrades(selectedComparisonValue1, grade) + ")"; break;
                        case 3: selectStr += " WHERE x.MeanGrade IN(" + GetGrades(selectedComparisonValue1, grade) + ")"; break;
                        case 4: selectStr += " WHERE x.MeanGrade IN(" + GetGrades(selectedComparisonValue1, grade) + ")"; break;
                        case 5: selectStr += " WHERE x.MeanGrade IN(" + GetGrades(selectedComparisonValue1, grade) + ")"; break;
                    }
                }

                if (selectedComparisonValue2 > 0)
                {
                    if (selectedComparisonValue1 > 0)
                        selectStr += " AND";
                    else
                        selectStr += " WHERE";
                    switch (selectedComparisonValue2)
                    {
                        case 1: selectStr += " x.TotalScore =" + score; break;
                        case 2: selectStr += " x.TotalScore >" + score; break;
                        case 3: selectStr += " x.TotalScore >=" + score; break;
                        case 4: selectStr += " x.TotalScore <" + score; break;
                        case 5: selectStr += " x.TotalScore <=" + score; break;
                    }
                }

                
                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private string GetGrades(int comparisonValue, string grade)
        {
            switch (comparisonValue)
            {
                case 1: return grade;
                case 2:
                    {
                        switch(grade)
                        {
                            case "A": return "'A'";
                            case "A-": return "'A'";
                            case "B+": return "'A-','A'";
                            case "B": return "'B+','A-','A'";
                            case "B-": return "'B','B+','A-','A'";
                            case "C+": return "'B-','B','B+','A-','A'";
                            case "C": return "'C+','B-','B','B+','A-','A'";
                            case "C-": return "'C','C+','B-','B','B+','A-','A'";
                            case "D+": return "'C-','C','C+','B-','B','B+','A-','A'";
                            case "D": return "'D+','C-','C','C+','B-','B','B+','A-','A'";
                            case "D-": return "'D','D+','C-','C','C+','B-','B','B+','A-','A'";
                            case "E": return "'D-','D','D+','C-','C','C+','B-','B','B+','A-','A'";
                        }
                        break;
                    }
                case 3:
                    {
                        switch (grade)
                        {
                            case "A": return "'A'";
                            case "A-": return "'A-','A'";
                            case "B+": return "'B+','A-','A'";
                            case "B": return "'B','B+','A-','A'";
                            case "B-": return "'B-','B','B+','A-','A'";
                            case "C+": return "'C+','B-','B','B+','A-','A'";
                            case "C": return "'C','C+','B-','B','B+','A-','A'";
                            case "C-": return "'C-','C','C+','B-','B','B+','A-','A'";
                            case "D+": return "'D+','C-','C','C+','B-','B','B+','A-','A'";
                            case "D": return "'D','D+','C-','C','C+','B-','B','B+','A-','A'";
                            case "D-": return "'D-','D','D+','C-','C','C+','B-','B','B+','A-','A'";
                            case "E": return "'E','D-','D','D+','C-','C','C+','B-','B','B+','A-','A'";
                        }
                        break;
                    }
                case 4:
                    {
                        switch (grade)
                        {
                            case "A": return "'E','D-','D','D+','C-','C','C+','B-','B','B+','A-'";
                            case "A-": return "'E','D-','D','D+','C-','C','C+','B-','B','B+'";
                            case "B+": return "'E','D-','D','D+','C-','C','C+','B-','B'";
                            case "B": return "'E','D-','D','D+','C-','C','C+','B-'";
                            case "B-": return "'E','D-','D','D+','C-','C','C+'";
                            case "C+": return "'E','D-','D','D+','C-','C'";
                            case "C": return "'E','D-','D','D+','C-'";
                            case "C-": return "'E','D-','D','D+'";
                            case "D+": return "'E','D-','D'";
                            case "D": return "'E','D-'";
                            case "D-": return "'E'";
                            case "E": return "'E'";
                        }
                        break;
                    }
                case 5:
                    {
                        switch (grade)
                        {
                            case "A": return "'E','D-','D','D+','C-','C','C'+,'B-','B','B+','A-','A'";
                            case "A-": return "'E','D-','D','D+','C'-,'C','C+','B-','B,'B+','A-'";
                            case "B+": return "'E','D-','D','D+','C-','C','C+','B-','B','B+'";
                            case "B": return "'E','D-','D','D+','C-','C','C+','B-','B'";
                            case "B-": return "'E','D-','D','D+','C-','C','C+','B-'";
                            case "C+": return "'E','D-','D','D+','C-','C','C+'";
                            case "C": return "'E','D-','D','D+','C-','C'";
                            case "C-": return "'E','D-','D','D+','C-'";
                            case "D+": return "'E','D-','D','D+'";
                            case "D": return "'E','D-','D'";
                            case "D-": return "'E','D-'";
                            case "E": return "'E'";
                        }
                        break;
                    } 
            }
            return "";
        }

        public int SelectedComparisonValue1
        {
            get { return selectedComparisonValue1; }

            set
            {
                if (value != selectedComparisonValue1)
                {
                    selectedComparisonValue1 = value;
                    NotifyPropertyChanged("SelectedComparisonValue1");
                }
            }
        }

        public int SelectedComparisonValue2
        {
            get { return selectedComparisonValue2; }

            set
            {
                if (value != selectedComparisonValue2)
                {
                    selectedComparisonValue2 = value;
                    NotifyPropertyChanged("SelectedComparisonValue2");
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

        public int ClassID
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

        public ExamModel SelectedExam
        {
            get { return this.selectedExam; }

            set
            {
                if (value != this.selectedExam)
                {
                    this.selectedExam = value;
                    NotifyPropertyChanged("SelectedExam");
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

        public ComparisonCollection Comparisons
        {
            get { return new ComparisonCollection(); }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get;
            private set;
        }

        public ObservableCollection<string> AllGrades
        {
            get;
            private set;
        }

        public ObservableCollection<ExamModel> AllExams
        {
            get { return allExams; }

            private set
            {
                if (value != allExams)
                {
                    allExams = value;
                    NotifyPropertyChanged("AllExams");
                }
            }
        }

        public Action<FixedDocument> ShowFullPreviewAction
        { get; set; }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }

        public ObservableCollection<ColumnModel> Columns
        {
            get { return columns; }

            private set
            {
                if (value != columns)
                {
                    columns = value;
                    NotifyPropertyChanged("Columns");
                }
            }
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

        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }
        public decimal OutOf {
            get { return outOf; }

            private set
            {
                if (value != outOf)
                {
                    outOf = value;
                    NotifyPropertyChanged("OutOf");
                }
            }
        }
    }
}
