﻿using Helper;
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
        
        ObservableCollection<ExamModel> allExams;
        private ExamModel selectedExam;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public EnterExamResultsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "ENTER RESULTS PER STUDENT";
            StudentSubjectSelection = new ObservableCollection<StudentSubjectSelectionEntryModel>();
            NewResult = new ExamResultStudentModel();
            AllExams = new ObservableCollection<ExamModel>();
            AllTerms = await DataAccess.GetAllTermsAsync();
            newResult.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    AllExams.Clear();
                    newResult.CheckErrors();
                    if ((newResult.StudentID != 0)&&(!newResult.HasErrors)&&selectedTerm!=null)
                    {
                        var s = await DataAccess.GetClassIDFromStudentID(newResult.StudentID);
                        AllExams = await DataAccess.GetExamsByClass(s,selectedTerm);
                        StudentSubjectSelection = (await DataAccess.GetStudentSubjectSelection(newResult.StudentID)).Entries;
                    }
                }

                

            };

            PropertyChanged += async(o, e) =>
                {
                    if (e.PropertyName == "SelectedTerm")
                    {
                        AllExams.Clear();
                        newResult.CheckErrors();
                        if ((newResult.StudentID != 0) && (!newResult.HasErrors) && selectedTerm != null)
                        {
                            var s = await DataAccess.GetClassIDFromStudentID(newResult.StudentID);
                            AllExams = await DataAccess.GetExamsByClass(s, selectedTerm);
                            StudentSubjectSelection = (await DataAccess.GetStudentSubjectSelection(newResult.StudentID)).Entries;
                        }
                    }
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
                        }
                    }
                };
        }

        protected override void CreateCommands()
        {
            AddAllSubjectsCommand = new RelayCommand(async o =>
            {
                await RefreshSubjectEntries();
            },
            o => CanReset());

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewExamResultAsync(newResult);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                            MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
                IsBusy = false;
            }, o => !IsBusy && CanSave());
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
        
        private bool CanReset()
        {
            newResult.CheckErrors();
            return !IsBusy&&!newResult.HasErrors && selectedExam != null;
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
            newResult.Entries = (await DataAccess.GetStudentExamResultAync(newResult.StudentID, newResult.ExamID, selectedExam.OutOf)).Entries;
            if (newResult.Entries.Count!=StudentSubjectSelection.Count)
            {
                foreach(var t in StudentSubjectSelection)
                    if(!newResult.Entries.Any(o=>o.SubjectID==t.SubjectID))
                        newResult.Entries.Add(new ExamResultSubjectEntryModel(t) { OutOf = selectedExam.OutOf});
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

        public ICommand AddAllSubjectsCommand
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
            SelectedExam = null;
            
        }
    }
}
