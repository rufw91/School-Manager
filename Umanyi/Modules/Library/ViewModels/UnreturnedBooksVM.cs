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
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class UnreturnedBooksVM: ViewModelBase
    {
        StudentBaseModel selectedStudent;
        ObservableCollection<BookModel> unreturnedBooks;
        private FixedDocument fd;
        public UnreturnedBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "UNRETURNED BOOKS";
            unreturnedBooks = new ObservableCollection<BookModel>();
            SelectedStudent = new StudentBaseModel();
            selectedStudent.CheckErrors();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    selectedStudent.CheckErrors();
                    unreturnedBooks.Clear();
                    if (!selectedStudent.HasErrors)
                        UnreturnedBooks = await DataAccess.GetUnreturnedBooksAsync(selectedStudent.StudentID);
                }
            };
        }
        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                var c = DataAccess.GetClass(await DataAccess.GetClassIDFromStudentID(selectedStudent.StudentID));
                UnreturnedBooksModel brm = new UnreturnedBooksModel();
                brm.StudentID = selectedStudent.StudentID;
                brm.NameOfStudent = selectedStudent.NameOfStudent;
                brm.ClassID = c.ClassID;
                brm.NameOfClass = c.NameOfClass;
                brm.Entries = unreturnedBooks;
                Document = DocumentHelper.GenerateDocument(brm);
            }, o => CanGenerate());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                var c = DataAccess.GetClass(await DataAccess.GetClassIDFromStudentID(selectedStudent.StudentID));
                UnreturnedBooksModel brm = new UnreturnedBooksModel();
                brm.StudentID = selectedStudent.StudentID;
                brm.NameOfStudent = selectedStudent.NameOfStudent;
                brm.ClassID = c.ClassID;
                brm.NameOfClass = c.NameOfClass;
                brm.Entries = unreturnedBooks;
                var dc = DocumentHelper.GenerateDocument(brm);
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(dc);
            }, o => CanGenerate());
        }

        private bool CanGenerate()
        {
            return unreturnedBooks.Count > 0;
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
        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
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

        public Action<FixedDocument> ShowFullPreviewAction
        { get; set; }

        public ICommand GenerateCommand
        { get; private set; }

        public ICommand FullPreviewCommand
        { get; private set; }

        public override void Reset()
        {
            selectedStudent.Reset();
            Document = null;
        }
    }
}
