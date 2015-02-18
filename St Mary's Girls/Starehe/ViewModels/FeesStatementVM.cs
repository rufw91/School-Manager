
using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
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
        }

        protected override void CreateCommands()
        {
            GenerateStatementCommand = new RelayCommand(async o =>
            {
                var s = await
                     DataAccess.GetFeesStatementAsync(statement.StudentID, statement.From, statement.To);
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

        public ICommand GenerateStatementCommand
        {
            get;
            private set;
        }

        private bool CanGnerateStatement()
        {
            statement.CheckErrors();
            return !statement.HasErrors && (statement.To > statement.From);
        }

        public override void Reset()
        {
            Statement.Reset();
            Document = null;
        }
    }
}

