using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class EnterExamResultsVM : ViewModelBase
    {
        ExamResultStudentModel newResult;
        ExamResultSubjectEntryModel newSubjectResult;
        ObservableCollection<ExamModel> allExams;
        ObservableCollection<ExamResultSubjectEntryModel> allSubjects;
        public EnterExamResultsVM()
        {
            InitVars();
            CreateCommands();

        }

        protected override void InitVars()
        {
            Title = "ENTER EXAM RESULTS";
            NewResult = new ExamResultStudentModel();
            SelectedSubject = new ExamResultSubjectEntryModel();
            SelectedExamID = 0;
            AllExams = new ObservableCollection<ExamModel>();
            AllSubjects = new ObservableCollection<ExamResultSubjectEntryModel>();
            newResult.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    if (newResult.StudentID != 0)
                    {
                        var s = await DataAccess.GetClassIDFromStudentID(newResult.StudentID);
                        AllExams = await DataAccess.GetExamsByClass(s);
                    }
                }

            };
        }

        protected override void CreateCommands()
        {
            AddSubjectResultCommand = new RelayCommand(o =>
            {
                newResult.Entries.Add(newSubjectResult);
                SelectedSubject = new ExamResultSubjectEntryModel();
                SelectedSubject = null;
            },
            o =>
            {
                return newSubjectResult != null && newSubjectResult.SubjectID > 0 &&
                    newSubjectResult.Score > 0 && !SubjectExists(newSubjectResult.SubjectID);
            });

            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewExamResultAsync(newResult);
                if (succ)
                    Reset();

            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            newResult.CheckErrors();
            return !newResult.HasErrors;
        }

        private bool SubjectExists(int ID)
        {
            bool exists = false;
            foreach (SubjectModel s in newResult.Entries)
                if (s.SubjectID == ID)
                {
                    exists = true;
                    break;
                }
            return exists;
        }

        public int SelectedExamID
        {
            get { return newResult.ExamID; }

            set
            {
                if (value != newResult.ExamID)
                {
                    newResult.ExamID = value;
                    NotifyPropertyChanged("SelectedExamID");
                    if (newResult.ExamID > 0)
                        RefreshSubjectEntries();
                    else
                        AllSubjects.Clear();
                }
            }
        }

        private async void RefreshSubjectEntries()
        {
            AllSubjects.Clear();
            var temp = (await DataAccess.GetExamAsync(newResult.ExamID)).Entries;
            foreach (SubjectModel sm in temp)
                AllSubjects.Add(new ExamResultSubjectEntryModel(sm));
            newResult.Entries = (await DataAccess.GetStudentExamResultAync(newResult.StudentID, newResult.ExamID)).Entries;
        }

        public ExamResultSubjectEntryModel SelectedSubject
        {
            get { return newSubjectResult; }

            set
            {
                if (value != newSubjectResult)
                {
                    newSubjectResult = value;
                    NotifyPropertyChanged("SelectedSubject");
                }
            }
        }

        public ObservableCollection<ExamModel> AllExams
        {
            get { return this.allExams; }

            private set
            {
                if (value != this.allExams)
                {
                    this.allExams = value;
                    NotifyPropertyChanged("AllExams");
                }
            }
        }

        public ObservableCollection<ExamResultSubjectEntryModel> AllSubjects
        {
            get { return this.allSubjects; }

            private set
            {
                if (value != this.allSubjects)
                {
                    this.allSubjects = value;
                    NotifyPropertyChanged("AllSubjects");
                }
            }
        }

        public ExamResultStudentModel NewResult
        {
            get { return this.newResult; }

            private set
            {
                if (value != this.newResult)
                {
                    this.newResult = value;
                    NotifyPropertyChanged("NewResult");
                }
            }
        }

        public ICommand AddSubjectResultCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            newResult.Reset();
            if (newSubjectResult != null)
                newSubjectResult.Reset();
            SelectedExamID = 0;
        }
    }
}
