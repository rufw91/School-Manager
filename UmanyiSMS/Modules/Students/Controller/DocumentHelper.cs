using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Students.Controller
{
    public class DocumentHelper:DocumentHelperBase
    {
        private DocumentHelper(object workObject)
            : base(workObject)
        {
        }

        public static FixedDocument GenerateDocument(object workObject)
        {
            new DocumentHelper(workObject);
            return Document;
        }

        protected override void AddDataToDocument()
        {            
            if (MyWorkObject is ClassStudentListModel)
                GenerateClassList();
            else if (MyWorkObject is LeavingCertificateModel)
                GenerateLeavingCert();
            else
            throw new ArgumentException();

        }

        protected override string GetResString()
        {            
            if (MyWorkObject is ClassStudentListModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Students/Resources/ClassList.xaml"));
            if (MyWorkObject is LeavingCertificateModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Students/Resources/LeavingCert.xaml"));

            return "";
        }

        protected override int GetNoOfPages()
        {            
            if (MyWorkObject is ClassStudentListModel)
                return 1;
            if (MyWorkObject is LeavingCertificateModel)
                return 1;            

            return 0;
        }

        protected override int GetItemsPerPage()
        {
            if (MyWorkObject is ClassStudentListModel)
                return 34;
            if (MyWorkObject is LeavingCertificateModel)
                return 1;

            return 0;
        }
                
        #region Class List

        private void AddCLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private void AddCLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 600, 85, pageNo);
        }

        private void AddCLStudent(StudentBaseModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 165 + pageRelativeIndex * 25;

            AddText(item.StudentID.ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.NameOfStudent, "Arial", fontsize, false, 0, Colors.Black, 300, yPos, pageNo);
        }
        private void AddCLStudents(ObservableCollection<StudentBaseModel> psi, int pageNo)
        {

            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCLStudent(psi[i], i, pageNo);
        }

        private void GenerateClassList()
        {
            ClassStudentListModel si = MyWorkObject as ClassStudentListModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddCLClass(si.NameOfClass, pageNo);
                AddCLDate(si.Date, pageNo);
                AddCLStudents(si.Entries, pageNo);
            }
        }
        #endregion

        #region LeavingCert

        private void AddLCDateEntered(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("D").ToUpper(), 12.5, true, 0, Colors.Black, 310, 355, pageNo);
        }
        private void AddLCStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 190, 275, pageNo);
        }
        private void AddLCNameOfStudent(string customerName, int pageNo)
        {
            AddText(customerName.ToUpperInvariant(), 14, true, 0, Colors.Black, 305, 315, pageNo);
        }
        private void AddLCDateIssued(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 230, 835, pageNo);
        }
        private void AddLCClassEntered(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 235, 395, pageNo);
        }
        private void AddLCDateLeft(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 515, 395, pageNo);
        }
        private void AddLCClassLeft(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 230, 435, pageNo);
        }
        private void AddLCOther(string className, int pageNo)
        {
            AddText(className.ToUpperInvariant(), 14, true, 0, Colors.Black, 340, 475, pageNo);
        }
        private void AddLCDateOfBirth(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd-MMM-yyyy").ToUpper(), 12.5, true, 0, Colors.Black, 390, 515, pageNo);
        }
        private void AddLCNationality(string nationality, int pageNo)
        {
            AddText(nationality.ToUpperInvariant(), 14, true, 0, Colors.Black, 255, 555, pageNo);
        }
        private void AddLCRemarks(string remarks, int pageNo)
        {
            AddTextWithWrap(remarks.ToUpperInvariant(), "Times New Roman", 512, 135, 14, true, 0, Colors.Black, 175, 600, pageNo);
        }
        private void GenerateLeavingCert()
        {
            LeavingCertificateModel si = MyWorkObject as LeavingCertificateModel;
            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
