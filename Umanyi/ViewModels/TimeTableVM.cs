using Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class TimeTableVM : ParentViewModel
    {
        public TimeTableVM()
        {
            Title = "TIME TABLE";
            TryAddChild(typeof(CurrentTimeTableFullVM));
            TryAddChild(typeof(ModifyTimeTableFullVM));            
        }
    }
}
