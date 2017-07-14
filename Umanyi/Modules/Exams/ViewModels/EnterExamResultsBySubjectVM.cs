

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Institution.Models;
using System.Collections.Specialized;
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class EnterExamResultsBySubjectVM:ViewModelBase
    {
        ObservableImmutableList<ExamModel> allExams;
        ObservableImmutableList<ExamResultStudentSubjectEntryModel> allSubjectResults;
        ObservableImmutableList<ExamResultSubjectEntryModel> allSubjects;
        private ObservableCollection<ClassModel> allClasses;
        private int selectedClassID;
        private int selectedExamID;
        private int selectedSubjectID;
        private string tutor;
        private ExamModel selectedExam;
        private bool isRemovingInvalid;
        private bool removeInvalid;
        public ObservableImmutableList<ExamResultStudentSubjectEntryModel> tempResults;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public EnterExamResultsBySubjectVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ENTER RESULTS BY SUBJECT";
            Tutor = "";
            allSubjectResults = new ObservableImmutableList<ExamResultStudentSubjectEntryModel>();
            allSubjects = new ObservableImmutableList<ExamResultSubjectEntryModel>();
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            SelectedExamID = 0;
            AllExams = new ObservableImmutableList<ExamModel>();
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
            allSubjectResults.CollectionChanged += (o, e) =>
            {
                if (isRemovingInvalid)
                    return;
                if (e.Action == NotifyCollectionChangedAction.Add)
                    foreach (SubjectModel i in e.NewItems)
                    {
                        if (i.MaximumScore > selectedExam.OutOf)
                        {
                            isRemovingInvalid = true;
                            allSubjectResults.Remove(i);
                            isRemovingInvalid = false;
                        }
                    }
            };
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "SelectedClassID" || e.PropertyName == "SelectedTerm")
                    {
                        allExams.Clear();
                        SelectedExamID = 0;
                        if (SelectedClassID != 0&&selectedTerm!=null)
                        {              
                            AllExams = new ObservableImmutableList<ExamModel>(await DataController.GetExamsByClass(selectedClassID,selectedTerm));
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
                        IsBusy = true;
                        if (SelectedSubjectID > 0)
                        {
                            AllSubjectResults = new ObservableImmutableList<ExamResultStudentSubjectEntryModel>(await DataController.GetStudentSubjectsResults(selectedClassID, selectedExamID, selectedSubjectID, selectedExam.OutOf));
                            tempResults = new ObservableImmutableList<ExamResultStudentSubjectEntryModel>(await DataController.GetStudentSubjectsResults(selectedClassID, selectedExamID, selectedSubjectID, selectedExam.OutOf));
                        }
                        IsBusy = false;
                    }
                    if (e.PropertyName=="Tutor")
                    {
                        if (allSubjectResults!=null)
                        foreach (var v in allSubjectResults)
                            v.Tutor = tutor;
                    }

                    if (e.PropertyName == "OutOf")
                    {
                        foreach(var f in AllSubjectResults)
                        {
                            f.MaximumScore = selectedExam.OutOf;
                        }
                    }
                };
        }


        private async Task RefreshSubjectEntries()
        {
            allSubjects.Clear();
            var temp = (await DataController.GetExamAsync(selectedExamID)).Entries;
            foreach (SubjectModel sm in temp)
                allSubjects.Add(new ExamResultSubjectEntryModel(sm));
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                CheckForChanges();
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
                bool succ = await DataController.SaveNewExamResultAsync(temp);
                IsBusy = false;
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();
                }
                IsBusy = false;

            }, o => !IsBusy && CanSave());
        }

        private void CheckForChanges()
        {
            if (tempResults.Any(o => !allSubjectResults.Any(a => a.StudentID == o.StudentID)))
            {
                var t = tempResults.Where(o => !allSubjectResults.Any(a => a.StudentID == o.StudentID));
                if (t != null && t.Count() > 0)
                {
                    int count =0;
                    string msg="The following results were removed:\r\n";
                    foreach(var i in t)
                    {
                        if (count > 20)
                        {
                            msg += ".....";
                            break;
                        }
                        msg += " -  Student: ["+i.StudentID+" - " + i.NameOfStudent + "] Score: [" + i.Score + "]\r\n";
                            count++;

                    }
                    msg += "Do you want to DELETE these student(s) results for selected subject?";
                    removeInvalid = (MessageBox.Show(msg, "Info", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes);
                    if (removeInvalid)
                    {
                        string remStr="";
                        foreach (var i in t)
                            remStr += "DELETE FROM [ExamResultDetail] WHERE SubjectID=" + i.SubjectID + " AND ExamResultID=" + i.ExamResultID + "\r\n"+
                                "IF NOT EXISTS (SELECT * FROM [ExamResultDetail] WHERE ExamResultID="+i.ExamResultID+")\r\n"+
                                "DELETE FROM [ExamResultHeader] WHERE ExamResultID=" + i.ExamResultID;                            
                        
                        bool succ = DataAccessHelper.Helper.ExecuteNonQuery(remStr);
                    }

                }
                else return;
            }
        }

        private bool CanSave()
        {
            return selectedClassID > 0 && selectedExamID > 0 && selectedSubjectID > 0 && allSubjectResults.Count > 0;
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

        public string Tutor
        {
            get { return this.tutor; }

            private set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
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
            SelectedSubjectID = 0;
            Tutor = "";
        }
    }
}
