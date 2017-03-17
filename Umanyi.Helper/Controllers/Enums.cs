
namespace UmanyiSMS.Lib
{
    public enum Gender
    { Male=0,Female=1}
    public enum UserRole
    {
        None, User, Teacher, Accounts, Deputy, Principal, SystemAdmin
    }

    public enum TaskStates
    {
        Idle = 0, PerformingTask = 1, TaskCompleteSucceeded = 2, TaskCompleteFailed = 3
    }

    public enum TransactionTypes
    {
        Debit, Credit, All
    }

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
