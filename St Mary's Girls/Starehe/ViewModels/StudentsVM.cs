using Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentsVM: ParentViewModel
    {
        public StudentsVM()
        {
            
            Title = "Students";
            TryAddChild(typeof(NewStudentVM));
            TryAddChild(typeof(StudentListVM));
            TryAddChild(typeof(ModifyStudentVM));
            TryAddChild(typeof(ClassListVM));
        }        
    }
}
