﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib
{
    public enum TaskState
    {
        Idle, Running, Complete
    }

    public enum ServerType
    {
        SqlServer, LocalDb
    }
}