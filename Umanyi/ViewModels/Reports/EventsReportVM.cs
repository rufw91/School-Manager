using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class EventsReportVM:ViewModelBase
    {
        private ObservableCollection<ColumnModel> columns;
        private FixedDocument document;
        private string name;
        private string subject;
        private string location;
        private DateTime? startDate;
        private DateTime? endDate;
        private TimeSpan? startTime;
        private TimeSpan? endTime;
        public EventsReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "EVENT REPORTS";
            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "Name","Name",1),
                new ColumnModel(true, "Subject","Subject",1),
                new ColumnModel(true, "Location","Location",1),
                new ColumnModel(true, "StartDateTime","Start Date/Time",1),
                new ColumnModel(true, "EndDateTime","End Date/Time",1)               
                
            };
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Library Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                Document = DocumentHelper.GenerateDocument(rt);
            }, o => CanRefresh());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                ReportModel rt = new ReportModel();
                rt.Title = "Library Report";
                rt.Entries = await GetEntries();
                rt.Columns = new ObservableCollection<ColumnModel>(columns.Where(ox => ox.IsSelected == true));
                var xdc = DocumentHelper.GenerateDocument(rt);
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(xdc);
            }, o => CanRefresh());

            ClearStartTimeCommand = new RelayCommand( o =>
            {
                StartTime = null;
            }, o => startTime.HasValue);
            ClearEndTimeCommand = new RelayCommand(o =>
            {
                EndTime = null;
            }, o => endTime.HasValue);
        }

        private Task<DataTable> GetEntries()
        {
            return Task.Factory.StartNew<DataTable>(() =>
            {
                string selectStr = "SELECT ";
                var t = columns.Where(ox => ox.IsSelected == true);
                foreach (var c in t)
                    selectStr += c.Name + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += " FROM [Institution].[Event]";
                selectStr += " WHERE Name LIKE '%" + Name + "%' AND Subject LIKE '%" + Subject + "%' AND Location LIKE '%" + Location +
                    "%' AND Message LIKE '%" + Subject + "%'";

                if (startDate.HasValue&&endDate.HasValue)                
                    selectStr += " AND StartDateTime>='" + startDate.Value.Date.ToString() + "' AND EndDateTime<='" +
                        endDate.Value.ToString("dd-MM-yyyy 23:59:998") + "'";
                if (startTime.HasValue&&endTime.HasValue)
                    selectStr += " AND CAST(StartDateTime AS TIME)>='" + startTime.Value.ToString() + "' AND CAST(EndDateTime AS TIME)<='" +
                        endTime.Value.ToString() + "'";
                    
                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private bool CanRefresh()
        {
            return !IsBusy && (endDate.HasValue && startDate.HasValue) ? endDate >= startDate : true && 
                (endTime.HasValue && startTime.HasValue) ? endTime >= startTime : true;
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

        public string Subject
        {
            get { return subject; }

            set
            {
                if (value != subject)
                {
                    subject = value;
                    NotifyPropertyChanged("Subject");
                }
            }
        }

        public string Location
        {
            get { return location; }

            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged("Location");
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

        public TimeSpan? StartTime
        {
            get { return startTime; }

            set
            {
                if (value != startTime)
                {
                    startTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        public TimeSpan? EndTime
        {
            get { return endTime; }

            set
            {
                if (value != endTime)
                {
                    endTime = value;
                    NotifyPropertyChanged("EndTime");
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

        public ICommand ClearEndTimeCommand
        {
            get;
            private set;
        }

        public ICommand ClearStartTimeCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }
    }
}
