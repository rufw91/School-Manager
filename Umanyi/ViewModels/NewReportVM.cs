using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewReportVM:ViewModelBase
    {
        public NewReportVM()
        {
            InitVars();
            CreateCommands();

        }
        protected override void InitVars()
        {
            Title = "SIMPLE REPORT";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
           
        }
    }
}
