using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ClassReportFormsVM: ViewModelBase
    {
        private int selectedClassID;
        private ObservableCollection<StudentTranscriptModel2> classTranscripts;
        private FixedDocument fd;
        private bool resultsIsReadOnly;
        private ObservableCollection<ExamWeightModel> exams;
        private string classTeacher;
        private DateTime openingDay;
        private DateTime closingDay;
        private string classTeacherComments;
        private string principalComments;
        public ClassReportFormsVM()
        {
            InitVars();
            CreateCommands();

        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(o =>
            {
                ClassTranscriptsModel2 cs = new ClassTranscriptsModel2() { Entries = classTranscripts };
                foreach(var f in cs.Entries)
                {
                    f.OpeningDay = openingDay;
                    f.ClosingDay = closingDay;
                    f.ClassTeacher = classTeacher;
                }
                Document = DocumentHelper.GenerateDocument(cs);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanGenerate());

            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                
                IEnumerable<ClassModel> classes =new List<ClassModel>();
                var ft= await DataAccess.GetAllCombinedClassesAsync();
                 var dx =ft.Where(o2=>o2.Entries.Any(o1=>o1.ClassID==selectedClassID));
                    classes = dx.ElementAt(0).Entries;

                ClassTranscripts = await DataAccess.GetClassTranscripts2Async(selectedClassID,exams,classes);
                ResultsIsReadOnly = true;
                IsBusy = false;
            }, o => CanRefresh());
        }

        protected async override void InitVars()
        {
            exams = new ObservableCollection<ExamWeightModel>();
            classTranscripts = new ObservableCollection<StudentTranscriptModel2>();
            Title = "CLASS REPORT FORMS";
            SelectedClassID = 0;
            OpeningDay = DateTime.Now;
            ClosingDay = DateTime.Now;
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "ClassTeacher")
                        foreach (var ed in classTranscripts)
                            ed.ClassTeacher = classTeacher;
                    if (e.PropertyName == "ClassTeacherComments")
                        foreach (var ed in classTranscripts)
                            ed.ClassTeacherComments = classTeacherComments;
                    if (e.PropertyName == "PrincipalComments")
                        foreach (var ed in classTranscripts)
                            ed.PrincipalComments = principalComments;
                    if ((e.PropertyName == "OpeningDay") || (e.PropertyName == "ClosingDay"))
                        foreach (var ed in classTranscripts)
                        {
                            ed.OpeningDay = openingDay;
                            ed.ClosingDay = closingDay;
                        }
                    if (e.PropertyName == "ClassTranscripts")
                    {
                            foreach (var ed in classTranscripts)
                                ed.ClassTeacher = classTeacher;
                            foreach (var ed in classTranscripts)
                                ed.ClassTeacherComments = classTeacherComments;
                            foreach (var ed in classTranscripts)
                                ed.PrincipalComments = principalComments;
                            foreach (var ed in classTranscripts)
                            {
                                ed.OpeningDay = openingDay;
                                ed.ClosingDay = closingDay;
                            }
                    }
                    if (e.PropertyName == "SelectedClassID")
                    {
                        exams.Clear();
                        classTranscripts.Clear();
                        ResultsIsReadOnly = false;
                        if (selectedClassID == 0)
                            return;
                        
                        var t = await DataAccess.GetExamsByClass(selectedClassID);
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
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }
        private bool CanRefresh()
        {
            decimal tot = 0;
            int count = 0;
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                count++;
            }
            return  tot == 100 && count <= 3 && count > 0;
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
        public ObservableCollection<StudentTranscriptModel2> ClassTranscripts
        {
            get { return classTranscripts; }
            set
            {
                if (value != classTranscripts)
                {
                    classTranscripts = value;
                    NotifyPropertyChanged("ClassTranscripts");

                }
            }
        }

        

        private bool CanGenerate()
        {
            return !IsBusy && classTranscripts.Count > 0;
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
    }
}
