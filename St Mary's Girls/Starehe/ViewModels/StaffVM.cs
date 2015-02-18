using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class StaffVM : ParentViewModel
    {
        public StaffVM()
        {         
            Title = "STAFF";
            TryAddChild(typeof(NewStaffVM));
            TryAddChild(typeof(StaffListVM));
            TryAddChild(typeof(ModifyStaffVM));           
        }
    }
}
