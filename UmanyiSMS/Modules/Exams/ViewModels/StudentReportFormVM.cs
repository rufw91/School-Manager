
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentReportFormVM: ViewModelBase
    {
        ReportFormModel reportForm;
        FixedDocument fd;
        ObservableCollection<ExamWeightModel> exams;
        bool resultsIsReadOnly;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public StudentReportFormVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "REPORT FORM";
            reportForm = new ReportFormModel();
            exams = new ObservableCollection<ExamWeightModel>();
            ResultsIsReadOnly = false;
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
            reportForm.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        exams.Clear();
                        reportForm.Clean();
                        ResultsIsReadOnly = false;
                        reportForm.CheckErrors();
                        if (!reportForm.HasErrors&& selectedTerm!=null)
                        {
                            reportForm.ClosingDay = selectedTerm.EndDate;
                            ResultsIsReadOnly = false;
                            var t = await DataController.GetExamsByClass(await Students.Controller.DataController.GetClassIDFromStudentID(reportForm.StudentID),selectedTerm);
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
                    }
                };
            PropertyChanged += async(o, e) =>
                {
                    if ( e.PropertyName == "SelectedTerm")
                    {
                        exams.Clear();
                        reportForm.Clean();
                        ResultsIsReadOnly = false;
                        reportForm.CheckErrors();
                        if (!reportForm.HasErrors && selectedTerm != null)
                        {
                            reportForm.ClosingDay = selectedTerm.EndDate;
                            ResultsIsReadOnly = false;
                            var t = await DataController.GetExamsByClass(await Students.Controller.DataController.GetClassIDFromStudentID(reportForm.StudentID), selectedTerm);
                            int count = 1;
                            foreach (var ex in t)
                            {
                                exams.Add(new ExamWeightModel()
                                {
                                    ExamID = ex.ExamID,
                                    NameOfExam = ex.NameOfExam,
                                    OutOf = ex.OutOf,
                                    Weight = count <= 3 ? ex.OutOf : 0,
								ShowInTranscript = count <= 3,
                                    Index = count>3?3:count
                                });
                                count++;
                            }
                        }
                    }
                };


        }

        protected override void CreateCommands()
        {
            PreviewCommand = new RelayCommand(o =>
            {
                Document = DocumentHelper.GenerateDocument(reportForm);
               
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());
            
            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                reportForm.CopyFrom(await DataController.GetStudentReportFormAsync(reportForm.StudentID, exams));
                reportForm.ClosingDay = selectedTerm.EndDate;
                ResultsIsReadOnly = true;
                IsBusy = false;
            }, o =>CanRefresh());
        }

        private bool CanRefresh()
        {
            decimal tot = 0;
            int count = 0;
            bool hasErr = false;

            List<int> indices = new List<int>(); 
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                hasErr = hasErr || (ed.ExamID == 0);
                indices.Add(ed.Index);
                count++;
            }
            bool repeated = indices.Count != indices.Distinct().Count();
                return !reportForm.HasErrors && tot == 100 && count <= 3 && count > 0 && !hasErr&&!repeated;
        }

        private bool CanSave()
        {
            decimal tot = 0;
            int count = 0;
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                count++;
            }

            return selectedTerm != null&& !reportForm.HasErrors && reportForm.SubjectEntries.Count > 0 && tot == 100 && count <= 3;
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

        public ReportFormModel ReportForm
        {
            get { return this.reportForm; }

            set
            {
                if (value != this.reportForm)
                {
                    this.reportForm = value;
                    NotifyPropertyChanged("ReportForm");
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

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand PreviewCommand
        { get; private set; }
        public ICommand SaveCommand
        { get; private set; }

        public ICommand RefreshCommand
        { get; private set; }
        public ICommand SaveAndPrintCommand
        { get; private set; }

        public override void Reset()
        {
            reportForm.Reset();
        }


    }
}
