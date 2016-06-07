using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SupplierStatementVM : ViewModelBase
    {
        SupplierStatementModel statement;
        FixedDocument fd;
        public SupplierStatementVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "SUPPLIER STATEMENT";
            statement = new SupplierStatementModel();
            statement.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "SupplierID")
                    statement.CheckErrors();
            };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(o =>
            {
                var doc = DocumentHelper.GenerateDocument(statement);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGnerateStatement() && Document != null);

            GenerateStatementCommand = new RelayCommand(async o =>
            {
                var s = await
                     DataAccess.GetSupplierStatementAsync(statement.SupplierID, statement.From, statement.To);
                statement.BalanceBroughtForward = s.BalanceBroughtForward;
                statement.TotalDue = s.TotalDue;
                statement.TotalPayments = s.TotalPayments;
                statement.TotalSales = s.TotalSales;
                statement.Transactions = s.Transactions;

                Document = DocumentHelper.GenerateDocument(statement);

            }, o => CanGnerateStatement());

        }

        public SupplierStatementModel Statement
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

