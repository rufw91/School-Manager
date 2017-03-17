using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ViewExamResultsVM : ViewModelBase
    {
        ExamResultStudentDisplayModel studentResult;        
        ExamModel selectedExam;
        ObservableCollection<ExamModel> allExams;
        bool canExec = false;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public ViewExamResultsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "VIEW EXAM RESULTS";
            StudentResult = new ExamResultStudentDisplayModel();
            AllExams = new ObservableCollection<ExamModel>();
            AllTerms = await DataController.GetAllTermsAsync();
            
            studentResult.PropertyChanged += OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
            
        }

        protected override void CreateCommands()
        {
            PrintTranscriptCommand = new RelayCommand(o =>
            {
                    IsBusy = true;
                    StudentExamResultModel st = DataController.GetStudentExamResult(studentResult);
                    IsBusy = false;
                    if (ShowStudentTranscriptAction != null)
                        ShowStudentTranscriptAction.Invoke(st);
                
               
            }, o => CanPrintResult());

            DisplayResultsCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                  var temp = new ExamResultStudentDisplayModel(await DataController.GetStudentExamResultAync(studentResult.StudentID, selectedExam.ExamID,selectedExam.OutOf));
                    StudentResult.Entries = temp.Entries;
                    StudentResult.ExamID = temp.ExamID;
                    StudentResult.ExamResultID = temp.ExamResultID;

                    StudentModel st = await DataController.GetStudentAsync(studentResult.StudentID);
                    studentResult.NameOfStudent = st.NameOfStudent;
                    studentResult.NameOfClass = (await DataController.GetClassAsync(st.ClassID)).NameOfClass;
                    studentResult.NameOfExam = selectedExam.NameOfExam;
               
                
                IsBusy = false;
            }, o => CanDisplayResults());
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "StudentID" || e.PropertyName == "SelectedTerm" || e.PropertyName == "SelectedExam")
                && studentResult.StudentID > 0)
            {
                studentResult.CheckErrors();
                if (!studentResult.HasErrors && selectedTerm != null)
                {
                    if (e.PropertyName != "SelectedExam")
                    {
                        int classID = await DataController.GetClassIDFromStudentID(studentResult.StudentID);
                        AllExams = await DataController.GetExamsByClass(classID, selectedTerm);
                    }
                    if (selectedExam != null)
                        RefreshView();
                }
            }

        }

        private bool CanPrintResult()
        {
                return (studentResult != null && studentResult.Entries.Count > 0)&&!IsBusy;
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
                studentResult.CheckErrors();
                return selectedTerm!=null&& selectedExam != null && selectedExam.ExamID > 0 &&
                      !studentResult.HasErrors&&!IsBusy;
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
            studentResult.Reset();
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
