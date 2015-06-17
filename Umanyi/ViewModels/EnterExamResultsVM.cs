using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class EnterExamResultsVM : ViewModelBase
    {
        ExamResultStudentModel newResult;
        ExamResultStudentModel tempResult;
        ExamResultSubjectEntryModel newSubjectResult;
        
        ObservableCollection<ExamModel> allExams;
        ObservableCollection<ExamResultSubjectEntryModel> allSubjects;
        private ExamModel selectedExam;
        public EnterExamResultsVM()
        {
            InitVars();
            CreateCommands();

        }

        protected override void InitVars()
        {
            Title = "ENTER EXAM RESULTS";
            StudentSubjectSelection = new ObservableCollection<StudentSubjectSelectionEntryModel>();
            NewResult = new ExamResultStudentModel();
            tempResult = new ExamResultStudentModel();
            SelectedSubject = new ExamResultSubjectEntryModel();
            AllExams = new ObservableCollection<ExamModel>();
            AllSubjects = new ObservableCollection<ExamResultSubjectEntryModel>();
            newResult.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    AllExams.Clear();
                    newResult.CheckErrors();
                    if ((newResult.StudentID != 0)&&(!newResult.HasErrors))
                    {
                        var s = await DataAccess.GetClassIDFromStudentID(newResult.StudentID);
                        AllExams = await DataAccess.GetExamsByClass(s);
                        StudentSubjectSelection = (await DataAccess.GetStudentSubjectSelection(newResult.StudentID)).Entries;
                    }
                }

                

            };

            PropertyChanged += async(o, e) =>
                {
                    if (e.PropertyName == "SelectedExam")
                    {
                        if (selectedExam != null)
                        {
                            newResult.ExamID = selectedExam.ExamID;

                            await RefreshSubjectEntries();
                        }
                        else
                        {
                            newResult.ExamID = 0;
                            AllSubjects.Clear();
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
                    !newSubjectResult.HasErrors && !SubjectExists(newSubjectResult.SubjectID) && 
                    StudentTakesSubject(newSubjectResult.SubjectID);
            });

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                CheckForChanges();
                bool succ = await DataAccess.SaveNewExamResultAsync(newResult);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                            MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
                IsBusy = false;
            }, o => !IsBusy && CanSave());
        }

        private void CheckForChanges()
        {
            bool removeInvalid=false;
            if (tempResult.Entries.Any(o => !newResult.Entries.Any(a => a.SubjectID == o.SubjectID)))
            {
                var t = tempResult.Entries.Where(o => !newResult.Entries.Any(a => a.SubjectID == o.SubjectID));
                if (t != null && t.Count() > 0)
                {
                    int count = 0;
                    string msg = "The following results were removed:\r\n";
                    foreach (var i in t)
                    {
                        if (count > 20)
                        {
                            msg += ".....";
                            break;
                        }
                        msg += " -  Subject: [" + i.NameOfSubject + "] Score: [" + i.Score + "]\r\n";
                        count++;

                    }
                    msg += "Do you want to DELETE these subject(s) results for selected student?";
                    removeInvalid = (MessageBox.Show(msg, "Info", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes);
                    if (removeInvalid)
                    {
                        string remStr = "";
                        foreach (var i in t)
                            remStr += "DELETE FROM [Institution].[ExamResultDetail] WHERE SubjectID=" + i.SubjectID + " AND ExamResultID=" + i.ExamResultID + "\r\n" +
                                "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultDetail] WHERE ExamResultID=" + i.ExamResultID + ")\r\n" +
                                "DELETE FROM [Institution].[ExamResultHeader] WHERE ExamResultID=" + i.ExamResultID;

                        bool succ = DataAccessHelper.ExecuteNonQuery(remStr);
                    }

                }
                else return;
            }
        }

        private bool CanSave()
        {
            newResult.CheckErrors();
            return !newResult.HasErrors && selectedExam!=null;
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

        private bool StudentTakesSubject(int subjectID)
        {
            return StudentSubjectSelection.Any(o => o.SubjectID == subjectID);
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

        private async Task RefreshSubjectEntries()
        {
            AllSubjects.Clear();
            var temp = (await DataAccess.GetExamAsync(newResult.ExamID)).Entries.Where(o => StudentSubjectSelection.Any(a => a.SubjectID == o.SubjectID));
            foreach (SubjectModel sm in temp)
                if (StudentTakesSubject(sm.SubjectID))
                    AllSubjects.Add(new ExamResultSubjectEntryModel(sm) { OutOf = selectedExam.OutOf });
            newResult.Entries = (await DataAccess.GetStudentExamResultAync(newResult.StudentID, newResult.ExamID, selectedExam.OutOf)).Entries;
            tempResult.Entries = (await DataAccess.GetStudentExamResultAync(newResult.StudentID, newResult.ExamID, selectedExam.OutOf)).Entries;
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

        private ObservableCollection<StudentSubjectSelectionEntryModel> StudentSubjectSelection
        {
            get;
            set;
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
            SelectedExam = null;
            
        }
    }
}
