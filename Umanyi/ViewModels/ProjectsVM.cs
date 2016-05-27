using Helper;
using System;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ProjectsVM : ParentViewModel
    {
        public ProjectsVM()
        {
            base.Title = "Projects";
            base.TryAddChild(typeof(NewProjectVM));
            base.TryAddChild(typeof(ProjectTimelineVM));
            base.TryAddChild(typeof(AllProjectsVM));
        }
    }
}
