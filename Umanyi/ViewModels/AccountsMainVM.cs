using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

            BackCommand = new RelayCommand(o =>
            {   
                if (CloseAction != null)
                {
                    CloseAction.Invoke();
                }
            }, o => true);            
        }

        public ICommand BackCommand
        { get; private set; }

        public Action CloseAction
        { get; internal set; }
    }
}
