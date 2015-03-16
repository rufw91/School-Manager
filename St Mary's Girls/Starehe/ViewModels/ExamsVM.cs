using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ExamsVM : ParentViewModel
    {
        
        public ExamsVM()
        {
            TryAddChild(typeof(NewExamVM));
            TryAddChild(typeof(EnterExamResultsVM));
            TryAddChild(typeof(EnterExamResultsBySubjectVM));            
            TryAddChild(typeof(ViewExamResultsVM));
            TryAddChild(typeof(StudentTranscriptVM));
            TryAddChild(typeof(ClassReportFormsVM));
            TryAddChild(typeof(AggregateResultsVM));
            
        }
    }
}
