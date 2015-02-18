using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class FeesVM : ParentViewModel
    {
        public FeesVM()
        {
            Title = "FEES";
            TryAddChild(typeof(NewFeesPaymentVM));
            TryAddChild(typeof(FeesStatementVM));
            TryAddChild(typeof(ViewFeesStructureVM));
            TryAddChild(typeof(SetFeesStructureVM));
            TryAddChild(typeof(ReprintReceiptVM));
            TryAddChild(typeof(FeesDefaultersVM));
        }
    }
}
