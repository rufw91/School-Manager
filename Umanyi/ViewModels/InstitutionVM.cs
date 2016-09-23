using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class InstitutionVM : ParentViewModel
    {
        public InstitutionVM()
        {
            Title = "SCHOOL";
            TryAddChild(typeof(InstitutionInfoVM));
            TryAddChild(typeof(InstitutionSetupVM));
            TryAddChild(typeof(AcademicYearSetupVM));
            TryAddChild(typeof(ClassesSetupVM));
            TryAddChild(typeof(InstitutionSubjectsSetupVM));
            TryAddChild(typeof(ModifySubjectsSetupVM));
            TryAddChild(typeof(ExamSetupVM));                        
            TryAddChild(typeof(NewDormitoryVM));
            TryAddChild(typeof(DormitoryMembersVM));
        }
    }
}
