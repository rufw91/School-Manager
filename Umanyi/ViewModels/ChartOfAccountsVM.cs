﻿using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class ChartOfAccountsVM : ViewModelBase
    {
        public ChartOfAccountsVM()
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

        protected override void InitVars()
        {
            Title = "CHART OF ACCOUNTS";
        }
    }
}