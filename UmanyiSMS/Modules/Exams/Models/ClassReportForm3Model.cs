using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassReportForm3Model : List<ReportForm3Model>
    {
        public void CopyFrom(ClassReportForm3Model classReportForms)
        {
            foreach (var t in classReportForms)
                Add(t);
        }
    }
}
