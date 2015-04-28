using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region LeavingCert

        private static void AddLCDateEntered(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("D").ToUpper(), 12.5, true, 0, Colors.Black, 310, 355, pageNo);
        }
        private static void AddLCStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 190, 275, pageNo);
        }
        private static void AddLCNameOfStudent(string customerName, int pageNo)
        {
            AddText(customerName.ToUpperInvariant(), 14, true, 0, Colors.Black, 305, 315, pageNo);
        }
        private static void AddLCDateIssued(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 230, 835, pageNo);
        }
        private static void AddLCClassEntered(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 235, 395, pageNo);
        }
        private static void AddLCDateLeft(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 515, 395, pageNo);
        }
        private static void AddLCClassLeft(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 230, 435, pageNo);
        }
        private static void AddLCOther(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 340, 475, pageNo);
        }
        private static void AddLCDateOfBirth(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 390, 515, pageNo);
        }
        private static void AddLCNationality(string nationality, int pageNo)
        {
            AddText(nationality.ToUpperInvariant(), 14, true, 0, Colors.Black, 255, 555, pageNo);
        }
        private static void AddLCRemarks(string remarks, int pageNo)
        {
            AddTextWithWrap(remarks.ToUpperInvariant(),"Times New Roman",512,135, 14, true, 0, Colors.Black, 175, 600, pageNo);
        }
        private static void GenerateLeavingCert()
        {
            LeavingCertificateModel si = myWorkObject as LeavingCertificateModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddLCDateEntered(si.DateOfAdmission, pageNo);
                AddLCStudentID(si.StudentID, pageNo);
                AddLCNameOfStudent(si.NameOfStudent, pageNo);
                AddLCClassEntered(si.ClassEntered, pageNo);
                AddLCRemarks(si.Remarks, pageNo);
                AddLCDateLeft(si.DateOfLeaving, pageNo);
                AddLCClassLeft(si.ClassLeft, pageNo);
                AddLCOther("FOUR", pageNo);
                AddLCNationality(si.Nationality, pageNo);
                AddLCDateOfBirth(si.DateOfBirth, pageNo);
                AddLCDateIssued(si.DateOfIssue, pageNo);
            }
        }
        #endregion
    }
}
