using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region ClassLeavingCerts
        private static void GenerateClassLeavingCerts()
        {
            ClassLeavingCertificatesModel si = myWorkObject as ClassLeavingCertificatesModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddLCDateEntered(si.Entries[pageNo].DateOfAdmission, pageNo);
                AddLCStudentID(si.Entries[pageNo].StudentID, pageNo);
                AddLCNameOfStudent(si.Entries[pageNo].NameOfStudent, pageNo);
                AddLCClassEntered(si.Entries[pageNo].ClassEntered, pageNo);
                AddLCRemarks(si.Entries[pageNo].Remarks, pageNo);
                AddLCDateLeft(si.Entries[pageNo].DateOfLeaving, pageNo);
                AddLCClassLeft(si.Entries[pageNo].ClassLeft, pageNo);
                AddLCOther("FOUR", pageNo);
                AddLCNationality(si.Entries[pageNo].Nationality, pageNo);
                AddLCDateOfBirth(si.Entries[pageNo].DateOfBirth, pageNo);
                AddLCDateIssued(si.Entries[pageNo].DateOfIssue, pageNo);
            }
        }
        #endregion
    }
}
