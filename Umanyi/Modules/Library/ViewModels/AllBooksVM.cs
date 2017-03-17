using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Library.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class AllBooksVM: ViewModelBase
    {
        public AllBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL BOOKS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
