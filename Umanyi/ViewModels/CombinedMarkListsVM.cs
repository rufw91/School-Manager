using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Windows;
using UmanyiSMS.Presentation;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class CombinedMarkListsVM: ViewModelBase
    {
        ObservableCollection<ExamWeightModel> exams;
        ExamResultClassDisplayModel classResult;
        private ObservableCollection<ClassModel> allClasses;
        private ClassModel selectedClass;
        private bool resultsIsReadOnly;
        private bool isInCombinedMode;
        private bool isInClassMode;
        private CombinedClassModel selectedCombinedClass;
        private ObservableCollection<CombinedClassModel> allCombinedClasses;
        public CombinedMarkListsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "COMBINED MARK LIST(S)";
            classResult = new ExamResultClassDisplayModel();
            exams = new ObservableCollection<ExamWeightModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            IsInClassMode = true;
            PropertyChanged += async (o, e) =>
                {
                    if (isInClassMode)
                    if (e.PropertyName == "SelectedClass")
                    {
                        exams.Clear();
                        ResultsIsReadOnly = false;
                        if ((selectedClass == null) || (selectedClass.ClassID == 0))
                            return;

                        var t = await DataAccess.GetExamsByClass(selectedClass.ClassID);
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
                    if(isInCombinedMode)
                    if (e.PropertyName == "SelectedCombinedClass")
                    {
                        exams.Clear();
                        ResultsIsReadOnly = false;
                        if ((selectedCombinedClass == null) || (selectedCombinedClass.Entries.Count == 0))
                            return;

                        var t = await DataAccess.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID);
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
        }



        protected override void CreateCommands()
        {
            ExportToExcelCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInClassMode)
                {
                    var temp = new ExamResultClassDisplayModel(await DataAccess.GetClassCombinedExamResultAsync(selectedClass.ClassID, exams));
                    classResult.Entries = temp.Entries;
                    classResult.ExamID = temp.ExamID;
                    classResult.ExamResultID = temp.ExamResultID;

                    ClassModel st = await DataAccess.GetClassAsync(classResult.ClassID);
                    classResult.NameOfClass = st.NameOfClass;

                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                    ClassExamResultModel stt = DataAccess.GetClassExamResult(classResult);

                    CommonCommands.ExportToExcelCommand.Execute(classResult.ResultTable);
                }

                if (isInCombinedMode)
                {
                    var temp = new ExamResultClassDisplayModel(await DataAccess.GetCombinedClassCombinedExamResultAsync(selectedCombinedClass.Entries, exams));

                    classResult.ExamID = temp.ExamID;
                    classResult.ExamResultID = temp.ExamResultID;
                    classResult.NameOfClass = selectedCombinedClass.Description;
                    classResult.Entries = temp.Entries;

                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                    ClassExamResultModel st = DataAccess.GetClassExamResult(classResult);
                    CommonCommands.ExportToExcelCommand.Execute(classResult.ResultTable);
                }
                IsBusy = false;
            }, o => CanGenerate());
            GenerateCommand = new RelayCommand(async o =>
            {

                IsBusy = true;
                if (isInClassMode)
                {                    
                    var temp = new ExamResultClassDisplayModel(await DataAccess.GetClassCombinedExamResultAsync(selectedClass.ClassID, exams));
                    classResult.Entries = temp.Entries;
                    classResult.ExamID = temp.ExamID;
                    classResult.ExamResultID = temp.ExamResultID;

                    ClassModel st = await DataAccess.GetClassAsync(classResult.ClassID);
                    classResult.NameOfClass = st.NameOfClass;

                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                    ClassExamResultModel stt = DataAccess.GetClassExamResult(classResult);
                    if (ShowClassTranscriptAction != null)
                        ShowClassTranscriptAction.Invoke(stt);
                }

                if (isInCombinedMode)
                {
                        var temp = new ExamResultClassDisplayModel(await DataAccess.GetCombinedClassCombinedExamResultAsync(selectedCombinedClass.Entries, exams));
                        
                            classResult.ExamID = temp.ExamID;
                            classResult.ExamResultID = temp.ExamResultID;
                            classResult.NameOfClass = selectedCombinedClass.Description;
                         classResult.Entries = temp.Entries;
                    
                    classResult.ResultTable = await ConvertClassResults(classResult.Entries.OrderByDescending(x => x.Total).ToList());
                    ClassExamResultModel st = DataAccess.GetClassExamResult(classResult);
                    if (ShowClassTranscriptAction != null)
                        ShowClassTranscriptAction.Invoke(st);
                }
                IsBusy = false;

            }, o => CanGenerate());
        }

        private async Task<DataTable> ConvertClassResults(List<ExamResultStudentModel> temp)
        {
            if (temp == null)
                return new DataTable();
            if (temp.Count == 0)
                return new DataTable();
            DataTable dt = new DataTable();
            ObservableCollection<SubjectModel> g;
            if (isInClassMode)
             g = await DataAccess.GetSubjectsRegistredToClassAsync(selectedClass.ClassID);
            else
                g = await DataAccess.GetSubjectsRegistredToClassAsync(selectedCombinedClass.Entries[0].ClassID);

            dt.Columns.Add(new DataColumn("Student ID"));
            dt.Columns.Add(new DataColumn("Name"));
            int subjectCount = 0;
            foreach (var d in g)
            {
                dt.Columns.Add(new DataColumn(d.NameOfSubject));
                subjectCount++;
            }
            dt.Columns.Add(new DataColumn("Grade"));
            dt.Columns.Add(new DataColumn("Total"));
            dt.Columns.Add(new DataColumn("Position"));
            DataRow dtr;
            ExamResultSubjectEntryModel f;
            int pos = 1;
            ExamResultStudentModel s;
            for (int x = 0; x < temp.Count; x++)
            {
                s = temp[x];
                dtr = dt.NewRow();
                dtr[0] = s.StudentID;
                dtr[1] = s.NameOfStudent;
                for (int i = 0; i < subjectCount; i++)
                {
                    f = s.Entries.FirstOrDefault(o => o.NameOfSubject == g[i].NameOfSubject);
                    dtr[i + 2] = (f != null) ? f.Score.ToString() : " - ";
                }
                dtr[subjectCount + 2] = s.MeanGrade;
                dtr[subjectCount + 3] = s.Total;
                dtr[subjectCount + 4] = pos;
                dt.Rows.Add(dtr);
                if ((temp.Count > x + 1) && (temp[x + 1].Total == s.Total))
                    continue;
                pos++;
            }
            return dt;

        }

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    classResult.Reset();
                }
            }
        }

        public bool IsInCombinedMode
        {
            get { return isInCombinedMode; }

            set
            {
                if (value != isInCombinedMode)
                {
                    isInCombinedMode = value;
                    NotifyPropertyChanged("IsInCombinedMode");
                    classResult.Reset();
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

        public ClassModel SelectedClass
        {
            get { return this.selectedClass; }

            set
            {
                if (value != this.selectedClass)
                {
                    this.selectedClass = value;
                    NotifyPropertyChanged("SelectedClass");
                }
            }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }

            set
            {
                if (value != selectedCombinedClass)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
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

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return this.allClasses; }

            private set
            {
                if (value != this.allClasses)
                {
                    this.allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return this.allCombinedClasses; }

            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        private bool CanGenerate()
        {
            
            decimal tot = 0;
            int count = 0;
            foreach (var ed in exams)
            {
                tot += ed.Weight;
                count++;
            }
            if (isInClassMode)
            return selectedClass!=null&&selectedClass.ClassID!=0&&tot == 100 && count <= 3 && count > 0;
            if (isInCombinedMode)
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count>0 && tot == 100 && count <= 3 && count > 0;
            return false;
        }

        public override void Reset()
        {
            
        }
        public Action<ClassExamResultModel> ShowClassTranscriptAction
        { get; set; }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public ICommand ExportToExcelCommand
        {
            get;
            private set;
        }
    }
}
