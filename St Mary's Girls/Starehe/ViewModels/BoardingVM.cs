using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class BoardingVM : ParentViewModel
    {
        public BoardingVM()
        {
            TryAddChild(typeof(NewDormitoryVM));
            TryAddChild(typeof(DormitoryMembersVM));
        }
    }
}
