using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AcademicReportModel:ReportModel
    {
        public AcademicReportModel()
        {
            Title = "Academic Report";
            Entries = new DataTable();
        }
        public override void Reset()
        {
        }
    }
}
