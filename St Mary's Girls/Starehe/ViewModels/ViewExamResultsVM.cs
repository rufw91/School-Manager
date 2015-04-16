using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewExamResultsVM : ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        bool isInCombinedMode;
        ExamResultStudentDisplayModel studentResult;
        ExamResultClassDisplayModel classResult;
        CombinedClassModel selectedCombinedClass;
        ExamModel selectedExam;
        ObservableImmutableList<ExamModel> allExams;
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
            AllExams = new ObservableImmutableList<ExamModel>();
            IsInStudentMode = true;
            studentResult.PropertyChanged += async (o, e) =>
            {
                if (isInStudentMode)
                    if (e.PropertyName == "StudentID")
                    {
                        RefreshView();
                        if (studentResult.StudentID > 0)
                            await RefreshAllExams();
                        else AllExams.Clear();
                    }

            };

            this.PropertyChanged += OnPropertyChanged;
            classResult.PropertyChanged += OnPropertyChanged;
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            NotifyPropertyChanged("AllCombinedClasses");
        }

        protected override void CreateCommands()
        {
            PrintAsReportFormCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                ClassStudentsExamResultModel st = new ClassStudentsExamResultModel();
                var sts = await DataAccess.GetClassStudents(classResult.ClassID);
                List<Task<ExamResultStudentDisplayModel>> l = new List<Task<ExamResultStudentDisplayModel>>();
                foreach (var f in sts)
                {
                    Task<ExamResultStudentDisplayModel> t = Task.Run<ExamResultStudentDisplayModel>(async () =>
                        {
                            ExamResultStudentDisplayModel temp = new ExamResultStudentDisplayModel();
                            var tx = new ExamResultStudentDisplayModel(await DataAccess.GetStudentExamResultAync(f.StudentID, selectedExam.ExamID));
                            temp.Entries = tx.Entries;
                            temp.ExamResultID = tx.ExamResultID;

                            temp.NameOfExam = selectedExam.NameOfExam;
                            temp.StudentID = f.StudentID;
                            temp.NameOfStudent = f.NameOfStudent;
                            temp.ExamID = selectedExam.ExamID;
                            temp.NameOfExam = selectedExam.NameOfExam;
                            var c = DataAccess.GetClass(await DataAccess.GetClassIDFromStudentID(f.StudentID));
                            temp.NameOfClass = c.NameOfClass;
                            return temp;
                        });

                    l.Add(t);

                }
                ObservableCollection<ExamResultStudentDisplayModel> res = new ObservableCollection<ExamResultStudentDisplayModel>(await Task.WhenAll<ExamResultStudentDisplayModel>(l));

                List<Task<StudentExamResultModel>> l2 = new List<Task<StudentExamResultModel>>();
                foreach (ExamResultStudentDisplayModel li in res)
                    l2.Add(Task.Run<StudentExamResultModel>(() => DataAccess.GetStudentExamResult(li)));
                StudentExamResultModel[] g = await Task.WhenAll<StudentExamResultModel>(l2);
                var r = g.OrderBy(k => k.StudentID);
                foreach (var d in r)
                    st.Entries.Add(d);
                IsBusy = false;
                if (ShowClassStudentsTranscriptAction != null)
                    ShowClassStudentsTranscriptAction.Invoke(st);
            }, o => isInClassMode && CanPrintResult());

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
                IsBusy = true;
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
                    var temp = new ExamResultClassDisplayModel(await DataAccess.GetClassExamResultAsync(classResult.ClassID, selectedExam.ExamID,selectedExam.OutOf));
                    ClassResult.Entries = temp.Entries;
                    ClassResult.ExamID = temp.ExamID;
                    ClassResult.ExamResultID = temp.ExamResultID;

                    ClassModel st = await DataAccess.GetClassAsync(classResult.ClassID);
                    classResult.NameOfClass = st.NameOfClass;
                    classResult.NameOfExam = selectedExam.NameOfExam;

                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                }

                if (isInCombinedMode)
                {
                    classResult.Entries.Clear();
                    ClassModel cs;
                    Debug.WriteLine("selectedCombinedClass count:" + selectedCombinedClass.Entries.Count);
                    for (int i = 0; i < selectedCombinedClass.Entries.Count;i++ )
                    {
                        cs = selectedCombinedClass.Entries[i];
                        Debug.WriteLine("ClassID:" + cs.ClassID + "   EXamID:" + selectedExam.ExamID);
                        Debug.WriteLine("Name:" + cs.NameOfClass);
                        var temp = new ExamResultClassDisplayModel(await DataAccess.GetClassExamResultAsync(cs.ClassID, selectedExam.ExamID,selectedExam.OutOf));
                        Debug.WriteLine("No of res:" + temp.Entries.Count);
                        foreach (var e in temp.Entries)
                        {
                            classResult.Entries.Add(e);                            
                        }

                        if (i == 0)
                        {
                            classResult.ExamID = temp.ExamID;
                            classResult.ExamResultID = temp.ExamResultID;
                            classResult.NameOfClass = selectedCombinedClass.Description;
                            classResult.NameOfExam = selectedExam.NameOfExam;
                            classResult.ClassID = cs.ClassID;
                        }                        
                    }
                    Debug.WriteLine("Number of Student Results count:" + classResult.Entries.Count);
                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                }
                IsBusy = false;
            }, o => CanDisplayResults());
        }
        
        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isInClassMode)
                if (e.PropertyName == "ClassID")
                {
                    await RefreshAllExams();
                    RefreshView();
                }
            if (e.PropertyName == "SelectedExam")
                RefreshView();

            if (e.PropertyName == "SelectedCombinedClass")
                if ((selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0))
                    await RefreshAllExams();
        }

        

        private bool CanPrintResult()
        {
            if (isInStudentMode)
                return (studentResult != null && studentResult.Entries.Count > 0)&&!IsBusy;
            else
                return (classResult != null && classResult.Entries.Count > 0)&&!IsBusy;
        }

        private async Task<DataTable> ConvertClassResults(List<ExamResultStudentModel> temp)
        {
            if (temp == null)
                return new DataTable();
            if (temp.Count == 0)
                return new DataTable();
            DataTable dt = new DataTable();
            var g = await DataAccess.GetSubjectsRegistredToClassAsync(classResult.ClassID);

            dt.Columns.Add(new DataColumn("Student ID"));
            dt.Columns.Add(new DataColumn("Name"));
            int subjectCount = 0;
            foreach (var d in g)
            {
                dt.Columns.Add(new DataColumn(d.NameOfSubject));
                subjectCount++;
            }
            dt.Columns.Add(new DataColumn("MeanGrade"));
            dt.Columns.Add(new DataColumn("Total"));
            dt.Columns.Add(new DataColumn("Position"));
            DataRow dtr;
            ExamResultSubjectEntryModel f;
            int pos = 1;
            ExamResultStudentModel s;
            for (int x = 0; x < temp.Count; x++)
            {
                s = temp[x];
                dtr = dt.NewRow();
                dtr[0] = s.StudentID;
                dtr[1] = s.NameOfStudent;
                for (int i = 0; i < subjectCount; i++)
                {
                    f = s.Entries.FirstOrDefault(o => o.NameOfSubject == g[i].NameOfSubject);
                    dtr[i + 2] = (f != null) ? f.Score.ToString() : " - ";
                }
                dtr[subjectCount + 2] = s.MeanGrade;
                dtr[subjectCount + 3] = s.Total;
                dtr[subjectCount + 4] = pos;
                dt.Rows.Add(dtr);
                if ((temp.Count > x + 1) && (temp[x + 1].Total == s.Total))
                    continue;
                pos++;
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
                      !studentResult.HasErrors&&!IsBusy;
            }

            if (isInClassMode)
                return selectedExam != null && selectedExam.ExamID > 0 && classResult.ClassID > 0&&!IsBusy;
            if (isInCombinedMode)
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0&&
                    selectedExam != null && selectedExam.ExamID > 0&&!IsBusy;

            return false;
        }

        public ObservableImmutableList<ExamModel> AllExams
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

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }

            set
            {
                if (value != selectedCombinedClass)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
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

        public bool IsInCombinedMode
        {
            get { return isInCombinedMode; }

            set
            {
                if (value != isInCombinedMode)
                {
                    isInCombinedMode = value;
                    NotifyPropertyChanged("IsInCombinedMode");
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

        private async Task RefreshAllExams()
        {
            if (isInStudentMode)
            {
                if (studentResult.StudentID == 0)
                    return;
                int classID = await DataAccess.GetClassIDFromStudentID(studentResult.StudentID);
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(classID));
                return;
            }
            if (isInClassMode)
            {
                if (classResult.ClassID == 0)
                    return;
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(classResult.ClassID));
                return;
            }
            if (isInCombinedMode)
            {
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID));
                return;
            }
        }

        public override void Reset()
        {
            studentResult.Reset();
            classResult.Reset();
        }

        public Action<ClassStudentsExamResultModel> ShowClassStudentsTranscriptAction
        { get; set; }

        public Action<StudentExamResultModel> ShowStudentTranscriptAction
        { get; set; }

        public Action<ClassExamResultModel> ShowClassTranscriptAction
        { get; set; }

        public  ICommand DisplayResultsCommand
        {
            get;
            private set;
        }

        public ICommand PrintAsReportFormCommand
        {
            get;
            private set;
        }

        public ICommand PrintTranscriptCommand
        {
            get;
            private set;
        }
        public ICommand ShowAllExamClassListCommand
        {
            get;
            private set;
        }
        public ICommand ShowAllExamCombinedClassListCommand
        {
            get;
            private set;
        }
        public ObservableCollection<ClassModel> AllClasses { get; private set; }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses { get; private set; }
    }
}
