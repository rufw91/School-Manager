

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class MarkListsVM: ViewModelBase    
    {
        bool isInClassMode;
        bool isInCombinedMode;
        ExamModel selectedExam;
        ExamResultClassModel classResult;
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<ExamModel> allExams;
        bool canExec = false;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public MarkListsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "MARK LIST(S)";
           
            ClassResult = new ExamResultClassModel();
            AllExams = new ObservableCollection<ExamModel>();
            IsInClassMode = true;
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
            classResult.PropertyChanged += OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
            AllCombinedClasses = await Institution.Controller.DataController.GetAllCombinedClassesAsync();
            NotifyPropertyChanged("AllCombinedClasses");
        }

        protected override void CreateCommands()
        {
            PrintAsReportFormCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                ExamResultClassDisplayModel st = await DataController.GetClassExamResultForTranscriptAsync(classResult.ClassID, selectedExam.ExamID, selectedExam.OutOf);

                IsBusy = false;
                if (ShowClassStudentsTranscriptAction != null)
                    ShowClassStudentsTranscriptAction.Invoke(st);
            }, o => false);

            PrintTranscriptCommand = new RelayCommand(o =>
            {
                    IsBusy = true;
                var st = new ExamResultClassModel(classResult);
                    IsBusy = false;
                    if (ShowClassTranscriptAction != null)
                        ShowClassTranscriptAction.Invoke(st);
                
            }, o => CanPrintResult());

            DisplayResultsCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                

                if (isInClassMode)
                {
                    var temp = await DataController.GetClassExamResultAsync(classResult.ClassID, selectedExam.ExamID, selectedExam.OutOf);
                    ClassResult.Entries = temp.Entries;
                    ClassResult.ExamID = temp.ExamID;
                    ClassResult.ExamResultID = temp.ExamResultID;

                    ClassModel st = await Institution.Controller.DataController.GetClassAsync(classResult.ClassID);
                    classResult.NameOfClass = st.NameOfClass;
                    classResult.NameOfExam = selectedExam.NameOfExam;

                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                }

                if (isInCombinedMode)
                {
                    classResult.Entries.Clear();
                    ClassModel cs;
                    for (int i = 0; i < selectedCombinedClass.Entries.Count; i++)
                    {
                        cs = selectedCombinedClass.Entries[i];
                        var temp = await DataController.GetClassExamResultAsync(cs.ClassID, selectedExam.ExamID, selectedExam.OutOf);

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
                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                }
                IsBusy = false;
            }, o => CanDisplayResults());
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isInClassMode)
            {
                if ((e.PropertyName == "ClassID" || e.PropertyName == "SelectedTerm" || e.PropertyName == "SelectedExam")
                    && classResult.ClassID > 0)
                {
                    if (e.PropertyName != "SelectedExam" && selectedTerm != null)
                    {
                        AllExams = await DataController.GetExamsByClass(classResult.ClassID, selectedTerm);
                    }
                    if (selectedExam != null)
                        RefreshView();
                }
            }
            if (isInCombinedMode)
            {
                if ((e.PropertyName == "SelectedCombinedClass" || e.PropertyName == "SelectedTerm" || e.PropertyName == "SelectedExam")
                    && selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0)
                {
                    if (e.PropertyName != "SelectedExam" && selectedTerm != null)
                        AllExams = await DataController.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID, selectedTerm);
                    if (selectedExam != null)
                        RefreshView();
                }
            }
        }

        private bool CanPrintResult()
        {
                return (classResult != null && classResult.Entries.Count > 0) && !IsBusy;
        }

        private async Task<DataTable> ConvertClassResults(List<ExamResultStudentModel> temp)
        {
            if (temp == null)
                return new DataTable();
            if (temp.Count == 0)
                return new DataTable();
            DataTable dt = new DataTable();
            var g = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();

            dt.Columns.Add(new DataColumn("Student ID"));
            dt.Columns.Add(new DataColumn("Name"));
            int subjectCount = 0;
            foreach (var d in g)
            {
                dt.Columns.Add(new DataColumn(d.NameOfSubject));
                subjectCount++;
            }
            dt.Columns.Add(new DataColumn("Grade"));
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
        public ObservableCollection<TermModel> AllTerms
        {
            get { return this.allTerms; }

            private set
            {
                if (value != this.allTerms)
                {
                    this.allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        public TermModel SelectedTerm
        {
            get { return this.selectedTerm; }

            set
            {
                if (value != this.selectedTerm)
                {
                    this.selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
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
           
            if (isInClassMode)
                return selectedTerm != null && selectedExam != null && selectedExam.ExamID > 0 && classResult.ClassID > 0 && !IsBusy;
            if (isInCombinedMode)
                return selectedTerm != null && selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
                    selectedExam != null && selectedExam.ExamID > 0 && !IsBusy;

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

        
        public ExamResultClassModel ClassResult
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

        public override void Reset()
        {
            classResult.Reset();
        }

        public Action<ExamResultClassDisplayModel> ShowClassStudentsTranscriptAction
        { get; set; }

        public Action<ExamResultStudentDisplayModel> ShowStudentTranscriptAction
        { get; set; }

        public Action<ExamResultClassModel> ShowClassTranscriptAction
        { get; set; }

        public ICommand DisplayResultsCommand
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
