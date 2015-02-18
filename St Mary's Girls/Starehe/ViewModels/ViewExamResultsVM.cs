using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Security.Permissions;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewExamResultsVM : ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        ExamResultStudentDisplayModel studentResult;
        ExamResultClassDisplayModel classResult;

        ExamModel selectedExam;
        ObservableCollection<ExamModel> allExams;
        bool canExec = false;
        public ViewExamResultsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "VIEW EXAM RESULTS";
            StudentResult = new ExamResultStudentDisplayModel();
            ClassResult = new ExamResultClassDisplayModel();
            AllExams = new ObservableCollection<ExamModel>();
            IsInStudentMode = true;
            studentResult.PropertyChanged += (o, e) =>
            {
                if (isInStudentMode)
                    if (e.PropertyName == "StudentID")
                    {
                        RefreshView();
                        if (studentResult.StudentID > 0)
                            RefreshAllExams();
                        else AllExams.Clear();
                    }

            };

            this.PropertyChanged += OnPropertyChanged;
            classResult.PropertyChanged += OnPropertyChanged;
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isInClassMode)
                if (e.PropertyName == "ClassID")
                {
                    RefreshAllExams();
                    RefreshView();
                }
            if (e.PropertyName == "SelectedExam")
                RefreshView();
        }

        protected override void CreateCommands()
        {
            PrintTranscriptCommand = new RelayCommand(o => 
            {
                if (isInStudentMode)
                {
                    StudentExamResultModel st = DataAccess.GetStudentExamResult(studentResult);
                    if (ShowStudentTranscriptAction != null)
                        ShowStudentTranscriptAction.Invoke(st);
                }
                else
                {
                    ClassExamResultModel st = DataAccess.GetClassExamResult(classResult);
                    if (ShowClassTranscriptAction != null)
                        ShowClassTranscriptAction.Invoke(st);
                }
            }, o => CanPrintResult());

            DisplayResultsCommand = new RelayCommand(async o =>
            {
                if (isInStudentMode)
                {
                    var temp = new ExamResultStudentDisplayModel(await DataAccess.GetStudentExamResultAync(studentResult.StudentID, selectedExam.ExamID));
                    StudentResult.Entries = temp.Entries;
                    StudentResult.ExamID = temp.ExamID;
                    StudentResult.ExamResultID = temp.ExamResultID;
                    
                    StudentModel st = await DataAccess.GetStudentAsync(studentResult.StudentID);
                    studentResult.NameOfStudent = st.NameOfStudent;
                    studentResult.NameOfClass = (await DataAccess.GetClassAsync(st.ClassID)).NameOfClass;
                    studentResult.NameOfExam = selectedExam.NameOfExam;
                }

                if (isInClassMode)
                {
                    var temp = new ExamResultClassDisplayModel(await DataAccess.GetClassExamResultAsync(classResult.ClassID, selectedExam.ExamID));
                    ClassResult.Entries = temp.Entries;
                    ClassResult.ExamID = temp.ExamID;
                    ClassResult.ExamResultID = temp.ExamResultID;

                    ClassModel st = await DataAccess.GetClassAsync(classResult.ClassID);
                    classResult.NameOfClass = st.NameOfClass;
                    classResult.NameOfExam = selectedExam.NameOfExam;

                    classResult.ResultTable = ConvertClassResults(classResult.Entries);
                }

            }, o => CanDisplayResults());
        }

        private bool CanPrintResult()
        {
            if (isInStudentMode)
                return (studentResult != null && studentResult.Entries.Count > 0);
            else
                return (classResult != null && classResult.Entries.Count > 0);
        }

        private DataTable ConvertClassResults(ObservableCollection<ExamResultStudentModel> temp)
        {
            if (temp == null)
                return new DataTable();
            if (temp.Count == 0)
                return new DataTable();
            DataTable dt = new DataTable();

            ExamResultStudentModel v = temp[0];

            dt.Columns.Add(new DataColumn("Student ID"));
            dt.Columns.Add(new DataColumn("Name"));
            int subjectCount = 0;
            foreach (ExamResultSubjectEntryModel d in v.Entries)
            {
                dt.Columns.Add(new DataColumn(d.NameOfSubject));
                subjectCount++;
            }
            DataRow dtr;

            foreach (ExamResultStudentModel s in temp)
            {
                dtr = dt.NewRow();
                dtr[0] = s.StudentID;
                dtr[1] = s.NameOfStudent;
                for (int i = 0; i < subjectCount; i++)
                    dtr[i + 2] = s.Entries[i].Score;
                dt.Rows.Add(dtr);
            }
            return dt;

        }

        private bool CanExecute
        {
            get { return canExec; }
            set
            {
                if (canExec != value)
                    canExec = value;
            }
        }

        private void RefreshView()
        {
            CanExecute = DisplayResultsCommand.CanExecute(null);

            if (canExec)
                DisplayResultsCommand.Execute(null);
        }

        private bool CanDisplayResults()
        {
            if (isInStudentMode)
            {
                studentResult.CheckErrors();
                return selectedExam != null && selectedExam.ExamID > 0 &&
                      !studentResult.HasErrors;
            }

            if (isInClassMode)
                return selectedExam != null && selectedExam.ExamID > 0 && classResult.ClassID > 0;

            return false;
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

        public ExamResultStudentDisplayModel StudentResult
        {
            get { return studentResult; }

            private set
            {
                if (value != studentResult)
                {
                    studentResult = value;
                    NotifyPropertyChanged("StudentResult");
                }
            }
        }

        public ExamResultClassDisplayModel ClassResult
        {
            get { return classResult; }

            private set
            {
                if (value != classResult)
                {
                    classResult = value;
                    NotifyPropertyChanged("ClassResult");
                }
            }
        }

        public bool IsInStudentMode
        {
            get { return isInStudentMode; }

            set
            {
                if (value != isInStudentMode)
                {
                    isInStudentMode = value;
                    NotifyPropertyChanged("IsInStudentMode");
                    StudentResult.Reset();
                    allExams.Clear();
                }
            }
        }

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    ClassResult.Reset();
                    allExams.Clear();
                }
            }
        }

        public ExamModel SelectedExam
        {
            get { return selectedExam; }

            set
            {
                if (value != selectedExam)
                {
                    selectedExam = value;
                    NotifyPropertyChanged("SelectedExam");
                }
            }
        }

        private async void RefreshAllExams()
        {
            if (isInStudentMode)
            {
                int classID = await DataAccess.GetClassIDFromStudentID(studentResult.StudentID);
                AllExams = await DataAccess.GetExamsByClass(classID);
                return;
            }
            if (isInClassMode)
            {
                AllExams = await DataAccess.GetExamsByClass(classResult.ClassID);
            }

        }

        public override void Reset()
        {
            studentResult.Reset();
            classResult.Reset();
        }

        public Action<StudentExamResultModel> ShowStudentTranscriptAction
        { get; set; }

        public Action<ClassExamResultModel> ShowClassTranscriptAction
        { get; set; }

        public  ICommand DisplayResultsCommand
        {
            get;
            private set;
        }

        public ICommand PrintTranscriptCommand
        {
            get;
            private set;
        }
        
        public ObservableCollection<ClassModel> AllClasses { get; private set; }
    }
}
