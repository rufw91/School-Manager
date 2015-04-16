using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class InstitutionVM : ParentViewModel
    {
        public InstitutionVM()
        {
            Title = "SCHOOL";
            TryAddChild(typeof(InstitutionInfoVM));
            TryAddChild(typeof(ClassesSetupVM));
            TryAddChild(typeof(SubjectsSetupVM));
            TryAddChild(typeof(SubjectSelectionSetupVM));
            TryAddChild(typeof(InstitutionSetupVM));
            TryAddChild(typeof(NewDormitoryVM));
            TryAddChild(typeof(DormitoryMembersVM));
        }
    }
}
