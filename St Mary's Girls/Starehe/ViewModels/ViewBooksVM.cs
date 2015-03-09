using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Data;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ViewBooksVM: ViewModelBase
    {
        CollectionViewSource collViewSource;
        string searchText;
        ObservableCollection<BookModel> allBooks;
        public ViewBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "VIEW BOOKS";
            AllBooks = await DataAccess.GetAllBooksAsync();
            collViewSource = new CollectionViewSource();
            collViewSource.Source = allBooks;
            NotifyPropertyChanged("CollViewSource");
        }

        protected override void CreateCommands() 
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                AllBooks = await DataAccess.GetAllBooksAsync();
                collViewSource = new CollectionViewSource();
                collViewSource.Source = allBooks;
                NotifyPropertyChanged("CollViewSource");
            }, o => true);
        }
        public ObservableCollection<BookModel> AllBooks
        {
            get { return this.allBooks; }

            set
            {
                if (value != this.allBooks)
                {
                    this.allBooks = value;
                    NotifyPropertyChanged("AllBooks");
                }
            }
        }
        public CollectionViewSource CollViewSource
        {
            get
            {
                return collViewSource;
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    searchText = value;
                    RenewFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
                else
                {
                    searchText = value;
                    if (searchText != null)
                        RemoveFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
            }
        }

        private void RenewFilter()
        {
            RemoveFilter();
            CollViewSource.Filter += new FilterEventHandler(Filter);
        }
        private void RemoveFilter()
        {
            CollViewSource.Filter -= new FilterEventHandler(Filter);
        }
        private void Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = false;
            var src = e.Item as BookModel;
            if (src == null)
            {
                e.Accepted = false;
            }
            else if (DataAccess.SearchAllBookProperties(src, SearchText))
                e.Accepted = true;
            else e.Accepted = false;
        }
        public ICommand RefreshCommand
        { get; private set; }
        public override void Reset()
        {
         
        }
    }
}
