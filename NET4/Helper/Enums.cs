﻿
namespace Helper
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
}
