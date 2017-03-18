
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Library.Controller;
using UmanyiSMS.Modules.Library.Models;

namespace UmanyiSMS.Modules.Library.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class AllUnreturnedBooksVM: ViewModelBase
    {
        ObservableCollection<UnreturnedBookModel> unreturnedBooks;
        private FixedDocument fd;
        public AllUnreturnedBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL UNRETURNED BOOKS";
            unreturnedBooks = new ObservableCollection<UnreturnedBookModel>();
        }


        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                unreturnedBooks = await DataController.GetUnreturnedBooksAsync();
                Document = DocumentHelper.GenerateDocument(new AllUnreturnedBooksModel(unreturnedBooks));
            }, o => CanGenerate());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                unreturnedBooks = await DataController.GetUnreturnedBooksAsync();
                var dc = DocumentHelper.GenerateDocument(new AllUnreturnedBooksModel(unreturnedBooks));
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(dc);
            });
        }

        private bool CanGenerate()
        {
            return true;
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

        public Action<FixedDocument> ShowFullPreviewAction
        { get; set; }

        public ICommand FullPreviewCommand
        { get; private set; }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }
    }
}
