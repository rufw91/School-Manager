using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class FinanceReportVM:ViewModelBase
    {
        private ObservableCollection<ColumnModel> columns;
        private FixedDocument document;
        private int studentID;
        private int selectedClassID;
        private int selectedComparisonValue;
        private decimal amount;
        public FinanceReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            AllClasses = new ObservableCollection<ClassModel>();
            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "s.StudentID","Student ID",1),
                new ColumnModel(true, "s.FirstName+' '+s.MiddleName+' '+s.LastName","Name Of Student",1),
                new ColumnModel(true, "c.NameOfClass","Class",1),
                new ColumnModel(true, "dbo.GetCurrentBalance(s.StudentID)","Balance",1)
            };
            SelectedClassID = 0;
            SelectedComparisonValue = 0;
            var f = await DataAccess.GetAllClassesAsync();
            AllClasses.Add(new ClassModel() { NameOfClass = "All", ClassID = 0 });
            foreach (ClassModel cs in f)
                AllClasses.Add(cs);
            NotifyPropertyChanged("AllClasses");
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Finance Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Finance Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                var xdc = DocumentHelper.GenerateDocument(rt);
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(xdc);
            }, o => CanRefresh());
        }

        private Task<DataTable> GetEntries()
        {
            return Task.Run<DataTable>(() =>
            {
                string selectStr = "SELECT ";
                var t = columns.Where(ox => ox.IsSelected == true);
                foreach (var c in t)
                    selectStr += c.Name + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += " FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID)";
                if (SelectedClassID > 0)
                    selectStr += " WHERE s.ClassID=" + selectedClassID;
                else if (studentID>0)
                    selectStr += " WHERE s.StudentID=" + studentID;
                
                if (selectedComparisonValue > 0)
                {
                    switch (selectedComparisonValue)
                    {
                        case 1: selectStr += " AND s.StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)=" + amount + ")"; break;
                        case 2: selectStr += " AND s.StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)>" + amount + ")"; break;
                        case 3: selectStr += " AND s.StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)>=" + amount + ")"; break;
                        case 4: selectStr += " AND s.StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)<" + amount + ")"; break;
                        case 5: selectStr += " AND s.StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)<=" + amount + ")"; break;
                    }

                }

                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get;
            private set;
        }

        public ComparisonCollection Comparisons
        {
            get { return new ComparisonCollection(); }
        }

        public int StudentID
        {
            get { return studentID; }

             set
            {
                if (value != studentID)
                {
                    studentID = value;
                    NotifyPropertyChanged("StudentID");
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

        public int SelectedComparisonValue
        {
            get { return selectedComparisonValue; }

            set
            {
                if (value != selectedComparisonValue)
                {
                    selectedComparisonValue = value;
                    NotifyPropertyChanged("SelectedComparisonValue");
                }
            }
        }

        public decimal Amount
        {
            get { return amount; }

            set
            {
                if (value != amount)
                {
                    amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public FixedDocument Document
        {
            get { return document; }

            private set
            {
                if (value != document)
                {
                    document = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public ObservableCollection<ColumnModel> Columns
        {
            get { return columns; }

            private set
            {
                if (value != columns)
                {
                    columns = value;
                    NotifyPropertyChanged("Columns");
                }
            }
        }

        public Action<FixedDocument> ShowFullPreviewAction
        { get; set; }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }



        public override void Reset()
        {
        }
    }
}
