﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AccountsIncomeStatementVM : ViewModelBase
    {
        private FixedDocument fd;
        public AccountsIncomeStatementVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                IncomeStatementModel im = await DataAccess.GetIncomeStatement(new DateTime(DateTime.Now.Year,1,1),new DateTime(DateTime.Now.Year,12,31));
                Document = DocumentHelper.GenerateDocument(im);
                IsBusy = false;
            }, o => true);
        }

        protected override void InitVars()
        {
            Title = "INCOME STATEMENT";
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }
    }
}