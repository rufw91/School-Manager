


using System;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class FeesStatementVM : ViewModelBase
    {
        FeesStatementModel statement;
        FixedDocument fd;
        public FeesStatementVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "FEES STATEMENT";
            statement = new FeesStatementModel();
            statement.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                    statement.CheckErrors();
            };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand=new RelayCommand(o =>
            {
                var doc = DocumentHelper.GenerateDocument(statement);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGnerateStatement()&&Document!=null);

            GenerateStatementCommand = new RelayCommand(async o =>
            {
                var s = await
                     DataController.GetFeesStatementAsync(statement.StudentID, statement.From, statement.To);
                statement.BalanceBroughtForward = s.BalanceBroughtForward;
                statement.TotalDue = s.TotalDue;
                statement.TotalPayments = s.TotalPayments;
                statement.TotalSales = s.TotalSales;
                statement.Transactions = s.Transactions;

                Document = DocumentHelper.GenerateDocument(statement);

            }, o => CanGnerateStatement());

        }

        public FeesStatementModel Statement
        {
            get { return statement; }

            set
            {
                if (value != statement)
                {
                    statement = value;
                    NotifyPropertyChanged("Statement");
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
        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }
        public ICommand GenerateStatementCommand
        {
            get;
            private set;
        }

        private bool CanGnerateStatement()
        {
            return !statement.HasErrors && (statement.To > statement.From);
        }
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public override void Reset()
        {
            Statement.Reset();
            Document = null;
        }
    }
}

