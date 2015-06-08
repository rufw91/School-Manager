using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class AggregateResultsVM: ViewModelBase
    {
        private FixedDocument fd;
        private ObservableCollection<ExamModel> allExams;
        private ExamModel selectedExam;
        private ClassModel selectedClass;
        private bool isInCombinedMode;
        private bool isInClassMode;
        private CombinedClassModel selectedCombinedClass;
        private ObservableCollection<ClassModel> allClasses;
        private ObservableCollection<CombinedClassModel> allCombinedClasses;

        public AggregateResultsVM()
        {
            InitVars();
            CreateCommands();            
        }

        protected async override void InitVars()
        {
            Title = "SUBJECT PERFOMANCE";            
            AllExams = new ObservableCollection<ExamModel>();
            IsInClassMode = true;
            AllClasses = await DataAccess.GetAllClassesAsync();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            PropertyChanged += async (o, e) =>
            {
                if (isInClassMode)
                    if ((e.PropertyName == "SelectedClass") && (selectedClass != null) && (selectedClass.ClassID > 0))
                        AllExams = await DataAccess.GetExamsByClass(selectedClass.ClassID);
                if (isInCombinedMode)
                    if ((e.PropertyName == "SelectedCombinedClass") && (selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0))
                        AllExams = await DataAccess.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID);
            };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInClassMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedClass, selectedExam);
                    var doc = DocumentHelper.GenerateDocument(fs);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(doc);
                }
                if (isInCombinedMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedCombinedClass, selectedExam);
                    var doc = DocumentHelper.GenerateDocument(fs);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(doc);
                }
                IsBusy = false;

            }, o => CanGenerate() && Document != null);
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInClassMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedClass, selectedExam);
                    Document = DocumentHelper.GenerateDocument(fs);
                }
                if (isInCombinedMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedCombinedClass, selectedExam);
                    Document = DocumentHelper.GenerateDocument(fs);
                }
                IsBusy = false;
            }, o => CanGenerate());
        }

        private bool CanGenerate()
        {
            if (isInClassMode)
                return selectedExam != null && selectedExam.ExamID > 0 && selectedClass.ClassID > 0 && !IsBusy;
            if (isInCombinedMode)
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
                    selectedExam != null && selectedExam.ExamID > 0 && !IsBusy;
            return false;
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

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    SelectedCombinedClass = null;
                    allExams.Clear();
                    NotifyPropertyChanged("IsInClassMode");                    
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
                    SelectedClass = null;
                    allExams.Clear();
                    NotifyPropertyChanged("IsInCombinedMode");
                    
                }
            }
        }

        public ClassModel SelectedClass
        {
            get { return selectedClass; }

            set
            {
                if (value != selectedClass)
                {
                    selectedClass = value;
                    NotifyPropertyChanged("SelectedClass");
                    
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
        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            private set
            {
                if (value != this.allClasses)
                {
                    this.allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }
        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }
        
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
        }
    }
}
