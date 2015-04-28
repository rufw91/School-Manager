using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ReportsVM : ParentViewModel
    {
        public ReportsVM()
        {
            TryAddChild(typeof(NewReportVM));
            TryAddChild(typeof(ReportsHomeVM));
            TryAddChild(typeof(LocalTutorialsVM));
            TryAddChild(typeof(OnlineResourcesVM));
        }
    }
}
