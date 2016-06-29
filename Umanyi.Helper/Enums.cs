﻿
namespace Helper
{
    public enum TransactionTypes
    {
        Debit, Credit, All
    }
    public enum Boardingtype { Boarder, DayScholar }

    public enum Comparisons { None = 0, IsEqualTo = 1, IsGreaterThan = 2, IsGreaterThanOrEqualTo = 3, IsLessThan = 4, IsLessThanOrEqualTo = 5 }

    public enum DonateTo
    {
        Fees,
        Projects
    }

    public enum AccountType
    { Asset, Liability, Equity, Expense, Revenue }

    public enum GeneralLedgerAccounts
    {
        Cash,
        Sales,
        AccountsPayable,
        AccountsReceivable,
        Salaries,
        OtherExpenses,
        OtherRevenue
    }
    
}