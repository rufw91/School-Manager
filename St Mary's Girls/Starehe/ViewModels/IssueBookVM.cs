using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class IssueBookVM: ViewModelBase
    {
        StudentSelectModel selectedStudent;
        BookModel addBook;
        BookModel removeBook;
        ObservableCollection<BookModel> thisIssue;
        CollectionViewSource collViewSource;
        string searchText;
        ObservableCollection<BookModel> allBooks;
        public IssueBookVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ISSUE BOOK";
            SelectedStudent = new StudentSelectModel();
            AllBooks = await DataAccess.GetAllBooksAsync();            
            collViewSource = new CollectionViewSource();
            collViewSource.Source = allBooks;
            NotifyPropertyChanged("CollViewSource");
            selectedStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                        selectedStudent.CheckErrors();
                };
            thisIssue = new ObservableCollection<BookModel>();
            NotifyPropertyChanged("ThisIssue");
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o=>
            {
                BookIssueModel bim = new BookIssueModel();
                bim.StudentID = selectedStudent.StudentID;
                bim.DateIssued = DateTime.Now;
                bim.Entries = thisIssue;
                bool succ = await DataAccess.SaveNewBookIssueAsync(bim);
                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            },o=>CanSave());

            RemoveCommand = new RelayCommand(o =>
            {
                thisIssue.Remove(removeBook);
            }, o => CanRemove()
            );
            AddCommand = new RelayCommand(o =>
            {
                if (!thisIssue.Contains(addBook))
                thisIssue.Add(addBook);
            }, o => CanAdd()
            );
        }

        private bool CanAdd()
        {
            return addBook != null;
        }

        private bool CanRemove()
        {
            return removeBook != null;
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
        private bool CanSave()
        {
            return !selectedStudent.HasErrors&&thisIssue.Count>0;
        }
        public CollectionViewSource CollViewSource
        {
            get
            {
                return collViewSource;
            }
        }
        public StudentSelectModel SelectedStudent
        {
            get { return this.selectedStudent; }

            set
            {
                if (value != this.selectedStudent)
                {
                    this.selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }
        public BookModel RemoveBook
        {
            get { return this.removeBook; }

            set
            {
                if (value != this.removeBook)
                {
                    this.removeBook = value;
                    NotifyPropertyChanged("RemoveBook");
                }
            }
        }
        public BookModel AddBook
        {
            get { return this.addBook; }

            set
            {
                if (value != this.addBook)
                {
                    this.addBook = value;
                    NotifyPropertyChanged("AddBook");
                }
            }
        }
        public ObservableCollection<BookModel> ThisIssue
        {
            get { return this.thisIssue; }

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
        public ICommand SaveCommand
        { get; private set; }
        public ICommand RemoveCommand
        { get; private set; }
        public ICommand AddCommand
        { get; private set; }
        public override void Reset()
        {
            selectedStudent.Reset();
            SearchText = "";
            thisIssue.Clear();
        }
    }
}
