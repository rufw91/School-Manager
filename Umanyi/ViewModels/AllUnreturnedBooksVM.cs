﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AllUnreturnedBooksVM: ViewModelBase
    {
        ObservableCollection<BookModel> unreturnedBooks;
        private FixedDocument fd;
        public AllUnreturnedBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL UNRETURNED BOOKS";
            unreturnedBooks = new ObservableCollection<BookModel>();
        }


        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                unreturnedBooks = await DataAccess.GetUnreturnedBooksAsync();
                Document = DocumentHelper.GenerateDocument(new AllUnreturnedBooksModel(unreturnedBooks));
            }, o => CanGenerate());

            FullPreviewCommand = new RelayCommand(async o =>
            {
                unreturnedBooks = await DataAccess.GetUnreturnedBooksAsync();
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
