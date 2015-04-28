using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class LibraryReportVM:ViewModelBase
    {
        private ObservableCollection<ColumnModel> columns;
        private FixedDocument document;
        private int selectedComparisonValue1;
        private int selectedComparisonValue2;
        private string isbn;
        private string name;
        private string author;
        private decimal price;
        private string publisher;
        private int noOfCopies;
        public LibraryReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            SelectedComparisonValue1 = 0;
            SelectedComparisonValue2 = 0;

            Columns = new ObservableCollection<ColumnModel>() 
            {
                new ColumnModel(true, "ISBN","ISBN",1),
                new ColumnModel(true, "Name","Title",1),
                new ColumnModel(true, "Author","Author",1),
                new ColumnModel(true, "Publisher","Publisher",1),
                new ColumnModel(true, "Price","Price",1)
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
                selectStr += " FROM [Institution].[Book]";
                selectStr += " WHERE ISBN LIKE '%" + ISBN + "%' AND Name LIKE '%" + Name + "%' AND Author LIKE '%" + Author + 
                    "%' AND Publisher LIKE '%" + Publisher + "%'";

                if (selectedComparisonValue1 > 0)
                {
                    selectStr += " AND";
                    switch (selectedComparisonValue1)
                    {
                        case 1: selectStr += " Price=" + price; break;
                        case 2: selectStr += " Price>" + price; break;
                        case 3: selectStr += " Price>=" + price; break;
                        case 4: selectStr += " Price<" + price; break;
                        case 5: selectStr += " Price<=" + price; break;
                    }
                }

                if (selectedComparisonValue2 > 0)
                {
                    selectStr += " AND";
                    switch (selectedComparisonValue2)
                    {
                        case 1: selectStr += " dbo.GetUnreturnedCopies(BookID)=" + noOfCopies; break;
                        case 2: selectStr += " dbo.GetUnreturnedCopies(BookID)>" + noOfCopies; break;
                        case 3: selectStr += " dbo.GetUnreturnedCopies(BookID)>=" + noOfCopies; break;
                        case 4: selectStr += " dbo.GetUnreturnedCopies(BookID)<" + noOfCopies; break;
                        case 5: selectStr += " dbo.GetUnreturnedCopies(BookID)<=" + noOfCopies; break;
                    }
                }
                return DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            });
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        public ComparisonCollection Comparisons
        {
            get { return new ComparisonCollection(); }
        }

        public int SelectedComparisonValue1
        {
            get { return selectedComparisonValue1; }

            set
            {
                if (value != selectedComparisonValue1)
                {
                    selectedComparisonValue1 = value;
                    NotifyPropertyChanged("SelectedComparisonValue1");
                }
            }
        }

        public int SelectedComparisonValue2
        {
            get { return selectedComparisonValue2; }

            set
            {
                if (value != selectedComparisonValue2)
                {
                    selectedComparisonValue2 = value;
                    NotifyPropertyChanged("SelectedComparisonValue2");
                }
            }
        }

        public string ISBN
        {
            get { return isbn; }

            set
            {
                if (value != isbn)
                {
                    isbn = value;
                    NotifyPropertyChanged("ISBN");
                }
            }
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

        public string Author
        {
            get { return author; }

            set
            {
                if (value != author)
                {
                    author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        public string Publisher
        {
            get { return publisher; }

            set
            {
                if (value != publisher)
                {
                    publisher = value;
                    NotifyPropertyChanged("Publisher");
                }
            }
        }

        public decimal Price
        {
            get { return price; }

            set
            {
                if (value != price)
                {
                    price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public int NoOfCopies
        {
            get { return noOfCopies; }

            set
            {
                if (value != noOfCopies)
                {
                    noOfCopies = value;
                    NotifyPropertyChanged("NoOfCopies");
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
