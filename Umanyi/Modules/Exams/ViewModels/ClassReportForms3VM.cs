
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Controller;
using System.Collections.Generic;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ClassReportForms3VM: ViewModelBase
    {
        private int selectedClassID;
        private ClassReportForm3Model classReportForms;
        private FixedDocument fd;
        private bool resultsIsReadOnly;
        private ObservableCollection<ExamWeightModel> exams;
        private string classTeacher;
        private DateTime openingDay;
        private DateTime closingDay;
        private string classTeacherComments;
        private string principalComments; 
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public ClassReportForms3VM()
        {
            InitVars();
            CreateCommands();

        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(o =>
            {
                ClassReportForm3Model cs = new ClassReportForm3Model();
                cs.CopyFrom(classReportForms);
                foreach(var f in cs)
                {
                    f.OpeningDay = openingDay;
                    f.ClosingDay = closingDay;
                }
                Document = DocumentHelper.GenerateDocument(cs);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanGenerate());

            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                
                ClassReportForms = await DataController.GetClassReportForms3Async(selectedClassID,exams,new Progress<OperationProgress>(DisplayProgress));
                
                ResultsIsReadOnly = true;
                IsBusy = false;
            }, o => CanRefresh());
        }

        private void DisplayProgress(OperationProgress progress)
        {
            OverallProgress = progress.OverallProgress;
            ProgressText = progress.ProgressText;
            NotifyPropertyChanged("OverallProgress");
            NotifyPropertyChanged("ProgressText");
        }

        protected async override void InitVars()
        {
            exams = new ObservableCollection<ExamWeightModel>();
            classReportForms = new ClassReportForm3Model();
            Title = "CLASS REPORT FORMS";
            SelectedClassID = 0;
            OpeningDay = DateTime.Now;
            ClosingDay = DateTime.Now;
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "ClassTeacherComments")
                        foreach (var ed in classReportForms)
                            ed.ClassTeacherComments = classTeacherComments;
                    if (e.PropertyName == "PrincipalComments")
                        foreach (var ed in classReportForms)
                            ed.PrincipalComments = principalComments;
                    if ((e.PropertyName == "OpeningDay") || (e.PropertyName == "ClosingDay"))
                        foreach (var ed in classReportForms)
                        {
                            ed.OpeningDay = openingDay;
                            ed.ClosingDay = closingDay;
                        }
                    if (e.PropertyName == "ClassReportForms")
                    {
                            foreach (var ed in classReportForms)
                                ed.ClassTeacherComments = classTeacherComments;
                            foreach (var ed in classReportForms)
                                ed.PrincipalComments = principalComments;
                            foreach (var ed in classReportForms)
                            {
                                ed.OpeningDay = openingDay;
                                ed.ClosingDay = closingDay;
                            }
                    }
                    if (e.PropertyName == "SelectedClassID" || e.PropertyName == "SelectedTerm")
                    {
                        exams.Clear();
                        classReportForms.Clear();
                        ResultsIsReadOnly = false;
                        if (selectedClassID == 0 || selectedTerm==null)
                            return;
                        
                        var t = await DataController.GetExamsByClass(selectedClassID,selectedTerm);
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
                    }
                };
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }
        private bool CanRefresh()
        {
            decimal tot = 0;
            int count = 0;
            List<int> indices = new List<int>();
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                indices.Add(ed.Index);
                count++;
            }
            bool repeated = indices.Count != indices.Distinct().Count();
            return  tot == 100 && count <= 3 && count > 0&&!repeated;
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

        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");

                }
            }
        }

        public string ClassTeacher
        {
            get { return classTeacher; }
            set
            {
                if (value != classTeacher)
                {
                    classTeacher = value;
                    NotifyPropertyChanged("ClassTeacher");

                }
            }
        }

        public string ClassTeacherComments
        {
            get { return classTeacherComments; }
            set
            {
                if (value != classTeacherComments)
                {
                    classTeacherComments = value;
                    NotifyPropertyChanged("ClassTeacherComments");

                }
            }
        }

        public string PrincipalComments
        {
            get { return principalComments; }
            set
            {
                if (value != principalComments)
                {
                    principalComments = value;
                    NotifyPropertyChanged("PrincipalComments");

                }
            }
        }

        public DateTime ClosingDay
        {
            get { return closingDay; }
            set
            {
                if (value != closingDay)
                {
                    closingDay = value;
                    NotifyPropertyChanged("ClosingDay");

                }
            }
        }

        public DateTime OpeningDay
        {
            get { return openingDay; }
            set
            {
                if (value != openingDay)
                {
                    openingDay = value;
                    NotifyPropertyChanged("OpeningDay");

                }
            }
        }

        public ObservableCollection<ExamWeightModel> Exams
        {
            get { return this.exams; }

            set
            {
                if (value != this.exams)
                {
                    this.exams = value;
                    NotifyPropertyChanged("Exams");
                }
            }
        }
        public ClassReportForm3Model ClassReportForms
        {
            get { return classReportForms; }
            set
            {
                if (value != classReportForms)
                {
                    classReportForms = value;
                    NotifyPropertyChanged("ClassReportForms");

                }
            }
        }

        

        private bool CanGenerate()
        {
            return !IsBusy && classReportForms.Count > 0;
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
        public ICommand PreviewCommand
        { get; private set; }
        public ICommand RefreshCommand
        { get; private set; }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        public override void Reset()
        {
            
        }

        public ObservableCollection<ClassModel> AllClasses { get; set; }

        public int OverallProgress { get; set; }

        public string ProgressText { get; set; }
    }
}
