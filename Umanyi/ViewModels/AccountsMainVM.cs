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
            TryAddChild(typeof(AccountsIncomeStatementVM));
            TryAddChild(typeof(AccountsBalanceSheetVM));
            TryAddChild(typeof(AccountsStatementOfCFVM));
            TryAddChild(typeof(ChartOfAccountsVM));
        }
    }
}
