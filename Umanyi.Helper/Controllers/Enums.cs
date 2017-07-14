
namespace UmanyiSMS.Lib
{
    public enum Gender
    { Male=0,Female=1}
    public enum UserRole
    {
        None=0, Teacher=1, Accounts=1, Deputy=3, Principal=4, SystemAdmin=5
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

}
