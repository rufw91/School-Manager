using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class SettingsVM: ParentViewModel
    {
        public SettingsVM()
        {
            Title = "SETTINGS";
            TryAddChild(typeof(GeneralSettingsVM));
            TryAddChild(typeof(SyncCloudVM));
            TryAddChild(typeof(NetworkOptionsVM));
            TryAddChild(typeof(AdvancedSettingsVM));
            TryAddChild(typeof(ChangeSAPasswordVM));
        }
    }
}
