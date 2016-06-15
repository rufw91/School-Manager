﻿using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AccountsVM : ParentViewModel
    {
        public AccountsVM()
        {
            TryAddChild(typeof(PrepareBudgetVM));
            TryAddChild(typeof(CurrentExpenditureVM));
            TryAddChild(typeof(AccountsBalanceSheetVM));
            TryAddChild(typeof(AccountsIncomeStatementVM));
            TryAddChild(typeof(AccountsBalanceSheetVM));
            TryAddChild(typeof(AccountsStatementOfCFVM));            
            TryAddChild(typeof(ChartOfAccountsVM));
            TryAddChild(typeof(NewItemCategoryVM));
            TryAddChild(typeof(RemoveAccountVM));
        }
    }
}