using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ExamsVM : ParentViewModel
    {
        
        public ExamsVM()
        {
            TryAddChild(typeof(NewExamVM));
            TryAddChild(typeof(ExamRegistrationVM));            
            TryAddChild(typeof(EnterExamResultsVM));
            TryAddChild(typeof(EnterExamResultsBySubjectVM));            
            TryAddChild(typeof(ViewExamResultsVM));
            TryAddChild(typeof(StudentTranscriptVM));
            TryAddChild(typeof(ClassReportFormsVM));
            TryAddChild(typeof(CombinedMarkListsVM));
            TryAddChild(typeof(AggregateResultsVM));
            TryAddChild(typeof(CombinedAggregateResultsVM));
            TryAddChild(typeof(RemoveExamVM));
        }
    }
}
