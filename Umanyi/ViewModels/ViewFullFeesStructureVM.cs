﻿using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewFullFeesStructureVM:ViewModelBase
    {
        public ViewFullFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "VIEW FULL FEES STRUCTURE";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
