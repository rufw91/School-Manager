

using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Library.Models;
using UmanyiSMS.Modules.Students.Models;
using UmanyiSMS.Modules.Library.Controller;

namespace UmanyiSMS.Modules.Library.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class IssueBookVM: ViewModelBase
    {
        StudentSelectModel selectedStudent;
        BookModel removeBook;
        ObservableCollection<BookModel> thisIssue;
        ObservableCollection<BookModel> allBooks;
        public IssueBookVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ISSUE BOOK";
            SelectedStudent = new StudentSelectModel();
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
                bool succ = await DataController.SaveNewBookIssueAsync(bim);
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
                IsBusy = true;
                if (FindBooksAction != null)
                {
                    FindBooksAction.Invoke();
                }
                IsBusy = false;
            }, o => CanAdd()
            );

        }

        private bool CanAdd()
        {
            return true;
        }

        private bool CanRemove()
        {
            return removeBook != null;
        }

        private bool CanSave()
        {
            return !selectedStudent.HasErrors&&thisIssue.Count>0;
        }

        public Action FindBooksAction
        { get; internal set; }


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

        public ObservableCollection<BookModel> ThisIssue
        {
            get { return this.thisIssue; }

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
            thisIssue.Clear();
        }
    }
}
