using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class EnterExamResultsBySubjectVM:ViewModelBase
    {
        ObservableImmutableList<ExamModel> allExams;
        ObservableImmutableList<ExamResultStudentSubjectEntryModel> allSubjectResults;
        ObservableImmutableList<ExamResultSubjectEntryModel> allSubjects;
        private ObservableCollection<ClassModel> allClasses;
        private int selectedClassID;
        private int selectedExamID;
        private int selectedSubjectID;
        public EnterExamResultsBySubjectVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ENTER RESULTS BY SUBJECT";
            allSubjectResults = new ObservableImmutableList<ExamResultStudentSubjectEntryModel>();
            allSubjects = new ObservableImmutableList<ExamResultSubjectEntryModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();
            SelectedExamID = 0;
            AllExams = new ObservableImmutableList<ExamModel>();
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "SelectedClassID")
                    {
                        allExams.Clear();
                        SelectedExamID = 0;
                        if (SelectedClassID != 0)
                        {                            
                            Debug.WriteLine("SelectedClassID is not Zero");
                            AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(selectedClassID));
                            Debug.WriteLine("Exams in class count:" + allExams.Count);
                        }
                        return;
                    }
                    if (e.PropertyName=="SelectedExamID")
                    {
                        allSubjects.Clear();
                        SelectedSubjectID = 0;
                        if (SelectedExamID > 0)                            
                            await RefreshSubjectEntries();
                    }
                    if (e.PropertyName == "SelectedSubjectID")
                    {
                        allSubjectResults.Clear();
                        if (SelectedSubjectID > 0)
                            AllSubjectResults = new ObservableImmutableList<ExamResultStudentSubjectEntryModel>(await DataAccess.GetStudentSubjectsResults(selectedClassID, selectedExamID, selectedSubjectID));                                                   
                    }
                };
        }

        private async Task RefreshSubjectEntries()
        {
            allSubjects.Clear();
            var temp = (await DataAccess.GetExamAsync(selectedExamID)).Entries;
            foreach (SubjectModel sm in temp)
                allSubjects.Add(new ExamResultSubjectEntryModel(sm));
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                ObservableCollection<ExamResultStudentModel> temp = new ObservableCollection<ExamResultStudentModel>();
                ExamResultStudentModel em;
                foreach(var d in allSubjectResults)
                {
                    em = new ExamResultStudentModel();
                    em.StudentID = d.StudentID;
                    d.SubjectID = selectedSubjectID;
                    em.ExamID = selectedExamID;
                    em.Entries.Add(d);
                    temp.Add(em);
                }
                IsBusy = true;
                bool succ = await DataAccess.SaveNewExamResultAsync(temp);
                IsBusy = false;
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();
                }


            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            return selectedClassID > 0 && selectedExamID > 0 && selectedSubjectID > 0 && allSubjectResults.Count > 0;
        }

        public ObservableImmutableList<ExamResultStudentSubjectEntryModel> AllSubjectResults
        {
            get { return this.allSubjectResults; }

            private set
            {
                if (value != this.allSubjectResults)
                {
                    this.allSubjectResults = value;
                    NotifyPropertyChanged("AllSubjectResults");
                }
            }
        }
        
        public ObservableImmutableList<ExamResultSubjectEntryModel> AllSubjects
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

        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (selectedClassID != value)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        public int SelectedSubjectID
        {
            get { return selectedSubjectID; }
            set
            {
                if (selectedSubjectID != value)
                {
                    selectedSubjectID = value;
                    NotifyPropertyChanged("SelectedSubjectID");
                }
            }
        }

        public int SelectedExamID
        {
            get { return selectedExamID; }

            set
            {
                if (value != selectedExamID)
                {
                    selectedExamID = value;
                    NotifyPropertyChanged("SelectedExamID");
                    
                }
            }
        }

        public ObservableImmutableList<ExamModel> AllExams
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

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            set
            {
                if (allClasses != value)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            SelectedClassID = 0;
        }
    }
}
