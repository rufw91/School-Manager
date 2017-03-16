using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentTranscriptVM: ViewModelBase
    {
        ReportFormModel reportForm;
        FixedDocument fd;
        ObservableCollection<ExamWeightModel> exams;
        bool resultsIsReadOnly;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public StudentTranscriptVM()
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
            AllTerms = await DataAccess.GetAllTermsAsync();
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
                            ResultsIsReadOnly = false;
                            var t = await DataAccess.GetExamsByClass(await DataAccess.GetClassIDFromStudentID(reportForm.StudentID),selectedTerm);
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
                            ResultsIsReadOnly = false;
                            var t = await DataAccess.GetExamsByClass(await DataAccess.GetClassIDFromStudentID(reportForm.StudentID), selectedTerm);
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


        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(o => 
            {
            }, o => CanSave());
            PreviewCommand = new RelayCommand(o =>
            {
                Document = DocumentHelper.GenerateDocument(reportForm);
               
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());
            SaveAndPrintCommand = new RelayCommand(o =>
            {                              
                Document = DocumentHelper.GenerateDocument(reportForm);
                Reset();
                IsBusy = false;
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());

            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                reportForm.CopyFrom(await DataAccess.GetStudentReportFormAsync(reportForm.StudentID, exams));
                
                ResultsIsReadOnly = true;
                IsBusy = false;
            }, o =>CanRefresh());
        }

        private bool CanRefresh()
        {
            decimal tot = 0;
            int count = 0;
            bool hasErr = false;
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                hasErr = hasErr || (ed.ExamID == 0);
                count++;
            }
            return !reportForm.HasErrors && tot == 100 && count <= 3 && count > 0 && !hasErr;
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
