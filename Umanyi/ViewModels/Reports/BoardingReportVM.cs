using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class BoardingReportVM:ViewModelBase
    {
        private ObservableCollection<ColumnModel> columns;
        private FixedDocument document;
        private bool showMembers;
        private int selectedComparisonValue;
        private int selectedDormitoryID;
        private int noOfMembers;
        public BoardingReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            ShowMembers = true;
            SelectedDormitoryID = 0;
            SelectedComparisonValue = 0;
            AllDorms = new ObservableCollection<DormModel>();
            var f = await DataAccess.GetAllDormsAsync();
            AllDorms.Add(new DormModel() { NameOfDormitory = "All", DormitoryID = 0 });
            foreach (DormModel cs in f)
                AllDorms.Add(cs);
            NotifyPropertyChanged("AllDorms");

            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "s.StudentID","Student ID",1),
                new ColumnModel(true, "s.FirstName+' '+s.MiddleName+' '+s.LastName","Name Of Student",1),
                new ColumnModel(true, "c.NameOfClass","Class",1),
                new ColumnModel(true, "d.NameOfDormitory","Dormitory",1)
            };

            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "ShowMembers")
                        SetColumns();

                };
        }

        private void SetColumns()
        {
            if (showMembers)
                Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "s.StudentID","Student ID",1),
                new ColumnModel(true, "s.FirstName+' '+s.MiddleName+' '+s.LastName","Name Of Student",1),
                new ColumnModel(true, "c.NameOfClass","Class",1),
                new ColumnModel(true, "d.NameOfDormitory","Dormitory",1)
            };
            else
                Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "d.NameOfDormitory","Dormitory",1),
                new ColumnModel(true, "(SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID)","No of Members",1)
            };
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Boarding Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Boarding Report";
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
                if (showMembers)
                    selectStr += " FROM [Institution].[Student] s INNER JOIN [Institution].[Dormitory] d ON(s.DormitoryID=d.DormitoryID)" +
                        "LEFT OUTER JOIN [Institution].[Class] c ON (s.ClassID=c.ClassID)";
                else
                    selectStr += " FROM [Institution].[Dormitory] d LEFT OUTER JOIN [Institution].[Student] s ON(d.DormitoryID=s.DormitoryID)";

                selectStr += (selectedDormitoryID > 0) ? " WHERE d.DormitoryID=" + selectedDormitoryID : "";

                if (selectedComparisonValue > 0)
                {
                    selectStr += (selectedDormitoryID > 0) ? " AND" : " WHERE";
                    switch (selectedComparisonValue)
                    {
                        case 1: selectStr += " ISNULL((SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID),0)=" + noOfMembers; break;
                        case 2: selectStr += " ISNULL((SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID),0)>" + noOfMembers; break;
                        case 3: selectStr += " ISNULL((SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID),0)>=" + noOfMembers; break;
                        case 4: selectStr += " ISNULL((SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID),0)<" + noOfMembers; break;
                        case 5: selectStr += " ISNULL((SELECT Count(*) FROM [Institution].[Student] WHERE DormitoryID=d.DormitoryID),0)<=" + noOfMembers; break;
                    }
                }

                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        public ObservableCollection<DormModel> AllDorms
        {
            get;
            private set;
        }

        public ComparisonCollection Comparisons
        {
            get { return new ComparisonCollection(); }
        }

        public bool ShowMembers
        {
            get { return showMembers; }

            set
            {
                if (value != showMembers)
                {
                    showMembers = value;
                    NotifyPropertyChanged("ShowMembers");
                }
            }
        }

        public int SelectedDormitoryID
        {
            get { return selectedDormitoryID; }

            set
            {
                if (value != selectedDormitoryID)
                {
                    selectedDormitoryID = value;
                    NotifyPropertyChanged("SelectedDormitoryID");
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

        public int NoOfMembers
        {
            get { return noOfMembers; }

            set
            {
                if (value != noOfMembers)
                {
                    noOfMembers = value;
                    NotifyPropertyChanged("NoOfMembers");
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
