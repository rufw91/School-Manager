using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassReportFormModel : List<ReportFormModel>
    {
        public void CopyFrom(ClassReportFormModel classReportForms)
        {
            foreach (var t in classReportForms)
                Add(t);
        }
    }
}
