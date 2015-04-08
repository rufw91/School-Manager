using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Data;

namespace Starehe.ViewModels
{
    public class StaffReportVM:ViewModelBase
    {
        private string name;
        private string nationalID;
        private string phoneNo;
        private DateTime? endDate;
        private DateTime? startDate;
        private ObservableCollection<ColumnModel> columns;
        private FixedDocument document;
        public StaffReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "Name","Name",1),
                new ColumnModel(true, "NationalID","National ID",1),
                new ColumnModel(true, "DateOfAdmission","Date Started",1),
                new ColumnModel(false, "PhoneNo","Phone No",1),
                new ColumnModel(true, "Email","Email",1),
                new ColumnModel(false, "Address","Address",1),
                new ColumnModel(false, "PostalCode","Postal Code",1),
                new ColumnModel(true, "City","City",1)
            };
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Staff Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Staff Report";
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
                selectStr += " FROM [Institution].[Staff] WHERE Name LIKE '%" + name +
                    "%' AND NationalID LIKE '%" + nationalID +
                    "%' AND PhoneNo LIKE '%" + phoneNo + "%'";

                if (startDate.HasValue && endDate.HasValue)
                    selectStr += " AND DateOfAdmission BETWEEN '" +
startDate.Value.Day.ToString() + "/" + startDate.Value.Month.ToString() + "/" + startDate.Value.Year.ToString() + " 00:00:00.000' AND '"
+ endDate.Value.Day.ToString() + "/" + endDate.Value.Month.ToString() + "/" + endDate.Value.Year.ToString() + " 23:59:59.998'";
                Log.I(startDate.HasValue + ":" + endDate.HasValue, null);
                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private bool CanRefresh()
        {
            return !IsBusy && (endDate.HasValue && startDate.HasValue) ? endDate >= startDate : true;
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

        public string NationalID
        {
            get { return nationalID; }

            set
            {
                if (value != nationalID)
                {
                    nationalID = value;
                    NotifyPropertyChanged("NationalID");
                }
            }
        }

        public string PhoneNo
        {
            get { return phoneNo; }

            set
            {
                if (value != phoneNo)
                {
                    phoneNo = value;
                    NotifyPropertyChanged("PhoneNo");
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
