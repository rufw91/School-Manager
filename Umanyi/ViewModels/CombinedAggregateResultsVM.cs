using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class CombinedAggregateResultsVM : ViewModelBase
    {
        private bool isInCombinedMode;
        private bool isInClassMode;
        private ObservableCollection<ClassModel> allClasses;
        private ObservableCollection<CombinedClassModel> allCombinedClasses;
        private ClassModel selectedClass;
        private CombinedClassModel selectedCombinedClass;
        private ObservableCollection<ExamWeightModel> exams;
        private bool resultsIsReadOnly;
        public CombinedAggregateResultsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "COMBINED SUBJECT PERFOMANCE";
            exams = new ObservableCollection<ExamWeightModel>();
            IsInClassMode = true;
            
            AllClasses = await DataAccess.GetAllClassesAsync();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            
            PropertyChanged += async (o, e) =>
            {
                if (isInClassMode)
                    if ((e.PropertyName == "SelectedClass") && (selectedClass != null) && (selectedClass.ClassID > 0))
                    {
                        exams.Clear();
                        ResultsIsReadOnly = false;

                        var t = await DataAccess.GetExamsByClass(selectedClass.ClassID);
                        int count = 1;

                        foreach (var ex in t)
                        {
                            exams.Add(new ExamWeightModel()
                            {
                                ExamID = ex.ExamID,
                                NameOfExam = ex.NameOfExam,
                                OutOf = ex.OutOf,
                                Weight = count <= 3 ? ex.OutOf : 0,
                                ShowInTranscript = count > 3 ? false : true,
                                Index = count
                            });
                            count++;
                        }
                        NotifyPropertyChanged("Exams");
                    }
                
                if (isInCombinedMode)
                
                    if ((e.PropertyName == "SelectedCombinedClass") && (selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0))
                    {
                        ResultsIsReadOnly = false;
                        if ((selectedCombinedClass == null) || (selectedCombinedClass.Entries.Count == 0))
                            return;

                        var t = await DataAccess.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID);
                        int count = 1;
                        foreach (var ex in t)
                        {
                            exams.Add(new ExamWeightModel()
                            {
                                ExamID = ex.ExamID,
                                NameOfExam = ex.NameOfExam,
                                OutOf = ex.OutOf,
                                Weight = count <= 3 ? ex.OutOf : 0,
                                ShowInTranscript = count > 3 ? false : true,
                                Index = count
                            });
                            count++;
                        }
                        NotifyPropertyChanged("Exams");
                    }
                
            };
        }

        protected override void CreateCommands()
        {            
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInClassMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedClass, exams);
                    var doc = DocumentHelper.GenerateDocument(fs);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(doc);
                }

                if (isInCombinedMode)
                {
                    AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedCombinedClass, exams);
                    var doc = DocumentHelper.GenerateDocument(fs);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(doc);
                }
                IsBusy = false;

            }, o => CanGenerate());
        }

        private bool CanGenerate()
        {
            decimal tot = 0;
            int count = 0;
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                count++;
            }
            if (isInClassMode)
                return selectedClass!=null && exams != null && tot == 100 && count <= 3 && count > 0 && selectedClass.ClassID > 0 && !IsBusy;
            if (isInCombinedMode)
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
                    exams != null && tot == 100 && count <= 3 && count > 0 && !IsBusy;
            return false;
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        public bool ResultsIsReadOnly
        {
            get { return this.resultsIsReadOnly; }

            set
            {
                if (value != this.resultsIsReadOnly)
                {
                    this.resultsIsReadOnly = value;
                    NotifyPropertyChanged("ResultsIsReadOnly");
                }
            }
        }

        public ClassModel SelectedClass
        {
            get { return this.selectedClass; }

            set
            {
                if (value != this.selectedClass)
                {
                    this.selectedClass = value;
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

        public ObservableCollection<ExamWeightModel> Exams
        {
            get { return this.exams; }

            private set
            {
                if (value != this.exams)
                {
                    this.exams = value;
                    NotifyPropertyChanged("Exams");
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
                    exams.Clear();
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
                    exams.Clear();
                }
            }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return this.allClasses; }

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
            get { return this.allCombinedClasses; }

            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
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
