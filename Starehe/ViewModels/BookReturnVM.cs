using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class BookReturnVM: ViewModelBase
    {
        StudentBaseModel selectedStudent;
        BookModel addBook;
        BookModel removeBook;
        ObservableCollection<BookModel> thisReturn;
        ObservableCollection<BookModel> unreturnedBooks;
        public BookReturnVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "BOOK RETURN";
            thisReturn = new ObservableCollection<BookModel>();
            unreturnedBooks = new ObservableCollection<BookModel>();
            SelectedStudent = new StudentBaseModel();
            selectedStudent.CheckErrors();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    selectedStudent.CheckErrors();
                    unreturnedBooks.Clear();
                    thisReturn.Clear();
                    if (!selectedStudent.HasErrors)
                    UnreturnedBooks = await DataAccess.GetUnreturnedBooksAsync(selectedStudent.StudentID);
                }
            };
            thisReturn = new ObservableCollection<BookModel>();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                BookReturnModel brm = new BookReturnModel();
                brm.StudentID = selectedStudent.StudentID;
                brm.DateReturned = DateTime.Now;
                brm.Entries = thisReturn;
                bool succ = await DataAccess.SaveNewBookReturnAsync(brm);
                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => CanSave());

            RemoveCommand = new RelayCommand(o =>
            {
                thisReturn.Remove(removeBook);
            }, o => CanRemove()
            );
            AddCommand = new RelayCommand(o =>
            {
                if (!thisReturn.Contains(addBook))
                    thisReturn.Add(addBook);
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
        private bool CanSave()
        {
            return !selectedStudent.HasErrors && thisReturn.Count > 0;
        }
        public StudentBaseModel SelectedStudent
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
        public ObservableCollection<BookModel> ThisReturn
        {
            get { return this.thisReturn; }

        }
        public ObservableCollection<BookModel> UnreturnedBooks
        {
            get { return this.unreturnedBooks; }

            set
            {
                if (value != this.unreturnedBooks)
                {
                    this.unreturnedBooks = value;
                    NotifyPropertyChanged("UnreturnedBooks");
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
            unreturnedBooks.Clear();
            selectedStudent.Reset();
            thisReturn.Clear();
        }
    }
}
