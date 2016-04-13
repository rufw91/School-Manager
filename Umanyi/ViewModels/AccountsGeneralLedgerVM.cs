using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace UmanyiSMS.ViewModels
{
    public class AccountsGeneralLedgerVM : ViewModelBase
    {
        ObservableCollection<AccountModel> accounts;
        private FixedDocument fd;

        public AccountsGeneralLedgerVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
        }

        protected async override void InitVars()
        {
            Title = "GENERAL LEDGER";
            Accounts = await DataAccess.GetGeneralLedgerAccountsAsync();
            Document = DocumentHelper.GenerateDocument(accounts);
        }

        public ObservableCollection<AccountModel> Accounts
        {
            get { return accounts; }
            private set
            {
                if (value != this.accounts)
                {
                    this.accounts = value;
                    NotifyPropertyChanged("Accounts");
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
    }
}
