using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Helper
{
    public static class DocumentHelper
    {
        static FixedDocument doc;
        static DocType docType;
        static int noOfPages;
        static int itemsPerPage;
        static object myWorkObject;
        enum DocType
        {
            Statement, Transcript, LeavingCert, FeesPayment, Balances, ClassList,Transcript2
        }
        private static void InitVars(object workObject)
        {
            myWorkObject = workObject;
            noOfPages = GetNoOfPages(workObject);
            doc = new FixedDocument();
            docType = GetDocType(workObject);
            itemsPerPage = GetItemsPerPage(docType);
        }

        public static FixedDocument GenerateDocument(object workObject)
        {
            if (workObject == null)
                throw new ArgumentNullException("workObject", "workObject cannot be null.");
            InitVars(workObject);
            AddPagesToDocument();
            AddDataToDocument();
            return doc;
        }

        private static FixedPage GetPage(DocType docType)
        {
            string resString = "";
            switch (docType)
            {
                case DocType.Statement: resString = Helper.Properties.Resources.Statement; break;
                case DocType.LeavingCert: resString = Helper.Properties.Resources.LeavingCert; break;
                case DocType.FeesPayment: resString = Helper.Properties.Resources.Receipt; break;
                case DocType.Transcript: resString = Helper.Properties.Resources.Transcript; break;
                case DocType.Balances: resString = Helper.Properties.Resources.Balances; break;
                case DocType.ClassList: resString = Helper.Properties.Resources.ClassList; break;
                case DocType.Transcript2: resString = Helper.Properties.Resources.Transcript2; break;
                    
            }
            StringReader stringReader = new StringReader(resString);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            FixedPage page = (FixedPage)XamlReader.Load(xmlReader);
            return page;
        }

        private static DocType GetDocType(object workObject)
        {
            if (workObject is LeavingCertificateModel)
                return DocType.LeavingCert;
            if (workObject is FeesStatementModel)
                return DocType.Statement;
            if (workObject is StudentTranscriptModel)
                return DocType.Transcript2;
            if (workObject is StudentExamResultModel)
                return DocType.Transcript;
            if (workObject is FeePaymentReceiptModel)
                return DocType.FeesPayment;
            if (workObject is ClassFeesDefaultModel)
                return DocType.Balances;
            if (workObject is ClassStudentListModel)
                return DocType.ClassList;
            
            throw new ArgumentException();
        }

        private static int GetItemsPerPage(DocType docType)
        {
            if (docType == DocType.Statement)
                return 22;
            return 31;
        }

        private static int GetNoOfPages(object workObject)
        {
            int totalNoOfItems = 0;
            DocType docType = GetDocType(workObject);
            if (docType == DocType.LeavingCert)
                return 1;
            if (docType == DocType.FeesPayment)
                return 1;
            if (docType == DocType.Transcript)
                return 1;
            if (docType == DocType.Transcript2)
                return 1;
            int itemsPerPage = GetItemsPerPage(docType);
            if (workObject is SaleModel)
                totalNoOfItems = (workObject as SaleModel).SaleItems.Count;
            if (workObject is FeesStatementModel)
                totalNoOfItems = (workObject as FeesStatementModel).Transactions.Count;
            if (workObject is ClassFeesDefaultModel)
            {
                totalNoOfItems = (workObject as ClassFeesDefaultModel).Entries.Count;
                itemsPerPage = 34;
            }

            if (workObject is ClassStudentListModel)
            {
                totalNoOfItems = (workObject as ClassStudentListModel).Entries.Count;
                itemsPerPage = 34;
            }
            return (totalNoOfItems % itemsPerPage) != 0 ?
                (totalNoOfItems / itemsPerPage) + 1 : (totalNoOfItems / itemsPerPage);
        }

        private static void AddText(string text, string fontFamily, double fontSize, bool isBold, double rotateAngle,
            Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily(fontFamily);
            text1.FontSize = fontSize;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;

            text1.Margin = new Thickness(left, top, 0, 0);

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }

        private static void AddText(string text, double fontSize, bool isBold, double rotateAngle,
            Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily("Times New Roman");
            text1.FontSize = fontSize;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;

            text1.Margin = new Thickness(left, top, 0, 0);

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }

        private static void AddText(string text, double left, double top, int pageNo)
        {
            AddText(text, 12, false, 0, Colors.Black, left, top, pageNo);
        }

        private static void AddPagesToDocument()
        {
            try
            {
                FixedPage p;
                for (int i = 0; i < noOfPages; i++)
                {
                    PageContent pageContent = new PageContent();
                    p = GetPage(docType);
                    ((IAddChild)pageContent).AddChild(p);
                    doc.Pages.Add(pageContent);
                }
            }
            catch { throw new InvalidOperationException(); }
        }

        private static void AddDataToDocument()
        {
            switch (docType)
            {
                case DocType.LeavingCert:
                    GenerateLeavingCert(); break;
                case DocType.Statement:
                    GenerateStatement(); break;
                case DocType.FeesPayment:
                    GenerateReceipt(); break;
                case DocType.Transcript: GenerateTranscript(); break;
                case DocType.Transcript2: GenerateTranscript2();  break;
                case DocType.Balances: GenerateBalanceList(); break;
                case DocType.ClassList: GenerateClassList();
                    break;
            }
        }
        #region Statement
        private static void AddSTPageNo(int pageNo, int totalPages)
        {
            AddText("Page " + (pageNo + 1).ToString() + " of " + totalPages, 12.5, false, 0, Colors.Black, 640, 40, pageNo);
        }
        private static void AddSTDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 640, 70, pageNo);
        }
        private static void AddSTStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 165, 180, pageNo);
        }
        private static void AddSTCustomerName(string customerName, int pageNo)
        {
            AddText(customerName.ToUpperInvariant(), 14, true, 0, Colors.Black, 165, 210, pageNo);
        }
        private static void AddSTPeriod(string period, int pageNo)
        {
            AddText(period, 16, true, 0, Colors.Black, 165, 240, pageNo);
        }
        private static void AddSTSales(decimal sales, int pageNo)
        {
            if (pageNo == 0)
                AddText(sales.ToString("N2"), 16, true, 0, Colors.Black, 280, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 280, 340, pageNo);
        }
        private static void AddSTPayments(decimal payments, int pageNo)
        {
            if (pageNo == 0)
                AddText(payments.ToString("N2"), 16, true, 0, Colors.Black, 448, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 448, 340, pageNo);
        }
        private static void AddSTTotal(decimal total, int pageNo)
        {
            if (pageNo == 0)
                AddText(total.ToString("N2"), 16, true, 0, Colors.Black, 645, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 645, 340, pageNo);
        }

        private static void AddSTTransaction(TransactionModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 405 + pageRelativeIndex * 25;
            if ((pageNo == 0) && (itemIndex == 0))
                AddText(item.TransactionAmt.ToString("N2"), 16, true, 0, Colors.Black, 70, 340, pageNo);
            else
            {
                AddText(item.TransactionDateTime.ToString("dd-MM-yyyy"), fontsize, false, 0, Colors.Black, 55, yPos, pageNo);
                AddText(item.TransactionID, fontsize, false, 0, Colors.Black, 220, yPos, pageNo);
                AddText((item.TransactionType == TransactionTypes.Credit) ? "Payment-" + item.TransactionID : "Sale-" + item.TransactionID,
                    fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
                AddText(item.TransactionAmt.ToString("N2"), fontsize, false, 0, Colors.Black, 665, yPos, pageNo);

            }
        }
        private static void AddSTTransactions(ObservableCollection<TransactionModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddSTTransaction(psi[i], i, pageNo);

        }

        private static void GenerateStatement()
        {
            FeesStatementModel si = myWorkObject as FeesStatementModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddSTPageNo(pageNo, noOfPages);
                AddSTDate(si.DateOfStatement, pageNo);
                AddSTStudentID(si.StudentID, pageNo);
                AddSTCustomerName(si.NameOfStudent, pageNo);
                AddSTPeriod(si.Period, pageNo);

                AddSTSales(si.TotalSales, pageNo);
                AddSTPayments(si.TotalPayments, pageNo);
                AddSTTotal(si.TotalDue, pageNo);

                AddSTTransactions(si.Transactions, pageNo);
            }
        }
        #endregion

        #region LeavingCert
        private static void GenerateLeavingCert()
        {
            LeavingCertificateModel lcm = myWorkObject as LeavingCertificateModel;

        }
        #endregion

        #region Receipt

        static double page2Offset = 561.28;
        private static void AddRCDate(DateTime dt, int pageNo)
        {            
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300, 220, pageNo);
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300+page2Offset, 220, pageNo);
        }
        private static void AddRCStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80, 231, pageNo);
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80 + page2Offset, 231, pageNo);
        }
        private static void AddRCStudentName(string studentName, int pageNo)
        {
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80, 249, pageNo);
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80 + page2Offset, 249, pageNo);
        }
        private static void AddRCClass(string nameOfclass, int pageNo)
        {
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80, 267, pageNo);
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80 + page2Offset, 267, pageNo);
        }
        private static void AddRCTerm(string term, int pageNo)
        {
            AddText(term, 14, true, 0, Colors.Black, 80, 285, pageNo);
            AddText(term, 14, true, 0, Colors.Black, 80 + page2Offset, 285, pageNo);
        }

        private static void AddRCFeesItem(FeesStructureEntryModel item, int itemIndex, int pageNo,bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 337 + pageRelativeIndex * 20;

            if (isAggregate)
            {
                AddText(item.Name, 16, true, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 250, yPos, pageNo);
                AddText(item.Name, 16, true, 0, Colors.Black, 30 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 250 + page2Offset, yPos, pageNo);
            }
            else
            {
                AddText(item.Name, fontsize, false, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 250, yPos, pageNo);
                AddText(item.Name, fontsize, false, 0, Colors.Black, 30 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 250 + page2Offset, yPos, pageNo);
            }
        }
        private static void AddRCFeesItems(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRCFeesItem(psi[i], i, pageNo, (startIndex>=endIndex-2));

        }

        private static void GenerateReceipt()
        {
            FeePaymentReceiptModel si = myWorkObject as FeePaymentReceiptModel;
            
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddRCDate(DateTime.Now, pageNo);
                AddRCStudentID(si.StudentID, pageNo);
                AddRCStudentName(si.NameOfStudent, pageNo);
                AddRCTerm(GetTerm(), pageNo);
                AddRCClass(si.NameOfClass, pageNo);
                AddRCFeesItems(si.Entries, pageNo);
            }
        }

        private static string GetTerm()
        {
            if (DateTime.Now.Month>= 1 && DateTime.Now.Month <=4)
                return "TERM I";
            else
                if (DateTime.Now.Month >=5  && DateTime.Now.Month <=8)
                    return "TERM II";
            else
                    return "TERM III";
        }
        #endregion

        #region Transcript
        
        private static void AddTRStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(),"Arial", 14, false, 0, Colors.Black, 95, 170, pageNo);
        }
        private static void AddTRName(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 247, 170, pageNo);
        }
        private static void AddTRClassName(string className, int pageNo)
        {
            AddText(className,"Arial", 14, true, 0, Colors.Black, 619, 170, pageNo);
        }
        private static void AddTRClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition,"Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }

        private static void AddTRTotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString(), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddTROverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 570, 580, pageNo);
        }
        private static void AddTRMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }

        private static void AddTRSubjectScore(StudentTranscriptSubjectModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 300, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 375, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
            AddText(item.Remarks, "Arial", fontsize, false, 0, Colors.Black, 480, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private static void AddTRSubjectScores(ObservableCollection<StudentTranscriptSubjectModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count-1; i++)
                AddTRSubjectScore(psi[i], i, pageNo);
        }

        private static void GenerateTranscript()
        {
            StudentExamResultModel si = myWorkObject as StudentExamResultModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddTRStudentID(si.StudentID, pageNo);
                AddTRName(si.NameOfStudent, pageNo);
                AddTRClassName(si.NameOfClass, pageNo);
                AddTRClassPosition(si.ClassPosition, pageNo);
                AddTROverAllPosition(si.OverAllPosition, pageNo);
                AddTRTotalMarks(si.TotalMarks,pageNo);
                AddTRMeanGrade(si.MeanGrade, pageNo);
                AddTRSubjectScores(si.Entries,pageNo);
            }
        }
        #endregion

        #region Transcript2

        private static void AddTR2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 95, 170, pageNo);
        }
        private static void AddTR2Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 247, 170, pageNo);
        }
        private static void AddTR2ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 619, 170, pageNo);
        }
        private static void AddTR2ClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }

        private static void AddTR2TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString(), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddT2ROverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 570, 580, pageNo);
        }
        private static void AddTR2MeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }

        private static void AddTR2SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            //AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            //AddText(item.Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 300, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 375, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
           // AddText(item.Remarks, "Arial", fontsize, false, 0, Colors.Black, 480, yPos, pageNo);
            //AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private static void AddTR2SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR2SubjectScore(psi[i], i, pageNo);
        }

        private static void GenerateTranscript2()
        {
            StudentTranscriptModel si = myWorkObject as StudentTranscriptModel;
            
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddTR2StudentID(si.StudentID, pageNo);
                AddTR2Name(si.NameOfStudent, pageNo);
                AddTR2ClassName(si.NameOfClass, pageNo);
                AddTR2ClassPosition(si.ClassPosition, pageNo);
                //AddTR2OverAllPosition(si.OverAllPosition, pageNo);
                AddTR2TotalMarks(si.TotalMarks, pageNo);
                AddTR2MeanGrade(si.MeanGrade, pageNo);
                AddTR2SubjectScores(si.Entries, pageNo);
            }
        }
        #endregion

        #region Balances

        private static void AddBLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private static void AddBLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 600, 85, pageNo);
        }

        private static void AddBLTotal(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 16, true, 0, Colors.Black, 80, 1050, pageNo);
        }

        private static void AddBLStudentBalance(StudentFeesDefaultModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 165 + pageRelativeIndex * 25;

            AddText(item.StudentID.ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.NameOfStudent, "Arial", fontsize, false, 0, Colors.Black, 175, yPos, pageNo);
            AddText(item.Balance.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, 445, yPos, pageNo);
            AddText(item.GuardianPhoneNo, "Arial", fontsize, false, 0, Colors.Black, 580, yPos, pageNo);
        }
        private static void AddBLStudentBalances(ObservableCollection<StudentFeesDefaultModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddBLStudentBalance(psi[i], i, pageNo);
        }

        private static void GenerateBalanceList()
        {
            ClassFeesDefaultModel si = myWorkObject as ClassFeesDefaultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddBLClass(si.NameOfClass, pageNo);
                AddBLDate(si.Date, pageNo);
                if (pageNo == 0)
                    AddBLTotal(si.Total, pageNo);
                AddBLStudentBalances(si.Entries, pageNo);
            }
        }
        #endregion

        #region Class List

        private static void AddCLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private static void AddCLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 600, 85, pageNo);
        }

        private static void AddCLStudent(StudentBaseModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 165 + pageRelativeIndex * 25;

            AddText(item.StudentID.ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.NameOfStudent, "Arial", fontsize, false, 0, Colors.Black, 300, yPos, pageNo);
        }
        private static void AddCLStudents(ObservableCollection<StudentBaseModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCLStudent(psi[i], i, pageNo);
        }

        private static void GenerateClassList()
        {
            ClassStudentListModel si = myWorkObject as ClassStudentListModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddCLClass(si.NameOfClass, pageNo);
                AddCLDate(si.Date, pageNo);
                AddCLStudents(si.Entries, pageNo);
            }
        }
        #endregion

    }

}
