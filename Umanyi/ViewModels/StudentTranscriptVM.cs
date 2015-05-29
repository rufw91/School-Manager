using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentTranscriptVM: ViewModelBase
    {
        StudentTranscriptModel transcript;
        FixedDocument fd;
        ObservableCollection<ExamWeightModel> exams;
        bool resultsIsReadOnly;
        public StudentTranscriptVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "REPORT FORM";
            Transcript = new StudentTranscriptModel();
            exams = new ObservableCollection<ExamWeightModel>();
            ResultsIsReadOnly = false;
            transcript.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        exams.Clear();
                        ResultsIsReadOnly = false;
                        transcript.CheckErrors();
                        if (!transcript.HasErrors)
                        {                            
                            ResultsIsReadOnly = false;
                            var t = await DataAccess.GetExamsByClass(await DataAccess.GetClassIDFromStudentID(transcript.StudentID));
                            int count = 1;
                            foreach(var ex in t)
                            {
                                exams.Add(new ExamWeightModel()
                                {
                                    ExamID = ex.ExamID,
                                    NameOfExam = ex.NameOfExam,
                                    OutOf = ex.OutOf,
                                    Weight = count <=3 ? ex.OutOf : 0,
                                    ShowInTranscript = count > 3 ? false : true,
                                    Index=count
                                });
                                count++;
                            }
                        }
                    }
                };
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o => 
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewStudentTranscript(transcript);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset(); 
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsBusy = false;
            }, o => CanSave());
            PreviewCommand = new RelayCommand(o =>
            {
                Document = DocumentHelper.GenerateDocument(transcript);
               
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());
            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewStudentTranscript(transcript);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);                
                Document = DocumentHelper.GenerateDocument(transcript);
                Reset();
                IsBusy = false;
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());

            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                var c = await DataAccess.GetClassIDFromStudentID(transcript.StudentID);

                IEnumerable<ClassModel> classes = new List<ClassModel>();
                var ft = await DataAccess.GetAllCombinedClassesAsync();
                var dx = ft.Where(o2 => o2.Entries.Any(o1 => o1.ClassID == c));
                    classes = dx.ElementAt(0).Entries;
                
                Transcript.CopyFrom(await DataAccess.GetStudentTranscript(transcript.StudentID,exams,classes));
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
            return !transcript.HasErrors && tot == 100 && count <= 3 && count > 0 && !hasErr;
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

            return !transcript.HasErrors && transcript.Entries.Count > 0 && tot == 100 && count <= 3;
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

        public StudentTranscriptModel Transcript
        {
            get { return this.transcript; }

            set
            {
                if (value != this.transcript)
                {
                    this.transcript = value;
                    NotifyPropertyChanged("Transcript");
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
            transcript.Reset();
        }
    }
}
