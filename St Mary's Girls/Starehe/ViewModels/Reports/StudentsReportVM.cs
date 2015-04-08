using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public sealed class StudentsReportVM:ViewModelBase
    {
        private string name;
        private string nameOfGuardian;
        private int selectedClassID;
        private decimal amount;
        private DateTime? startDate;
        private DateTime? endDate;
        private FixedDocument document;
        private ObservableCollection<ColumnModel> columns;
        private int selectedComparisonValue;
        public StudentsReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            AllClasses = new ObservableCollection<ClassModel>();
            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "FirstName","First Name",1),
                new ColumnModel(true, "MiddleName","Middle Name",1),
                new ColumnModel(true, "LastName","Last Name",1),
                new ColumnModel(false, "DateOfBirth","Date Of Birth",1),
                new ColumnModel(true, "DateOfAdmission","Date Admitted",1),
                new ColumnModel(false, "NameOfGuardian","Guardian",1),
                new ColumnModel(true, "GuardianPhoneNo","Guardian PhoneNo",1),
                new ColumnModel(true, "Address","Address",1),
                 new ColumnModel(false, "PostalCode","Postal Code",1),
                new ColumnModel(true, "City","City",1),
                new ColumnModel(false, "IsActive","Is Active",1),
                new ColumnModel(true, "PreviousBalance","Previous Balance",1),
                new ColumnModel(false, "PreviousInstitution","Previous Institution",1)
            };
            SelectedClassID = 0;
            SelectedComparisonValue = 0;
            var f = await DataAccess.GetAllClassesAsync();
            AllClasses.Add(new ClassModel() { NameOfClass = "None", ClassID = 0 });
            foreach (ClassModel cs in f)
                AllClasses.Add(cs);
            NotifyPropertyChanged("AllClasses");
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Student(s) Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Student(s) Report";
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
                    selectStr += " FROM [Institution].[Student] WHERE EXISTS (SELECT * FROM [Institution].[Student] WHERE FirstName LIKE '%" + name +
                        "%' OR MiddleName LIKE '%" + name +
                        "%' OR LastName LIKE '%" + name + "%') AND NameOfGuardian LIKE '%" + nameOfGuardian + "%'";
                    if (SelectedClassID > 0)
                        selectStr += " AND ClassID=" + selectedClassID;
                    if (selectedComparisonValue>0)
                    {
                        switch (selectedComparisonValue)
                        {
                            case 1: selectStr += " AND StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)=" + amount+")";  break;
                            case 2: selectStr += " AND StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)>" + amount + ")"; break;
                            case 3: selectStr += " AND StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)>=" + amount + ")"; break;
                            case 4: selectStr += " AND StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)<" + amount + ")"; break;
                            case 5: selectStr += " AND StudentID IN(Select StudentID FROM [Institution].[Student] WHERE dbo.GetCurrentBalance(StudentID)<=" + amount + ")"; break;                           
                        }
                        
                    }

                    if (startDate.HasValue && endDate.HasValue)
                        selectStr += " AND DateOfAdmission BETWEEN '" +
   startDate.Value.Day.ToString() + "/" + startDate.Value.Month.ToString() + "/" + startDate.Value.Year.ToString() + " 00:00:00.000' AND '"
   + endDate.Value.Day.ToString() + "/" + endDate.Value.Month.ToString() + "/" + endDate.Value.Year.ToString() + " 23:59:59.998'";

                    return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                });
        }

        private bool CanRefresh()
        {
            return !IsBusy && (endDate.HasValue && startDate.HasValue) ? endDate >= startDate : true;
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

        public string Name
        {
            get { return name; }

            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
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

        public string NameOfGuardian
        {
            get { return nameOfGuardian; }

            set
            {
                if (value != nameOfGuardian)
                {
                    nameOfGuardian = value;
                    NotifyPropertyChanged("NameOfGuardian");
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

        public DateTime? StartDate
        {
            get { return startDate; }

            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }

        public DateTime? EndDate
        {
            get { return endDate; }

            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    NotifyPropertyChanged("EndDate");
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
