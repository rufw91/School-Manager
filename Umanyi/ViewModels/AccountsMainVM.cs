using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class AccountsMainVM : ParentViewModel
    {
        public AccountsMainVM()
        {
            TryAddChild(typeof(AccountsTransactionsVM));
            TryAddChild(typeof(AccountsJournalVM));
            TryAddChild(typeof(AdjustJournalVM));
            TryAddChild(typeof(AccountsGeneralLedgerVM));
            TryAddChild(typeof(AccountsTrialBalanceVM));
            TryAddChild(typeof(AccountsBalanceSheetVM));
            TryAddChild(typeof(AccountsIncomeStatementVM));
        }
    }
}
