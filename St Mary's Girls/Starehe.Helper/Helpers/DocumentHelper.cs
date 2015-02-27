using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
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
            Statement, Transcript, LeavingCert, FeesPayment, Balances, ClassList, Transcript2, Voucher,ClassMarkList,AggregateResult
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
        private static void InitVars(object workObject)
        {
            myWorkObject = workObject;
            docType = GetDocType(workObject);
            itemsPerPage = GetItemsPerPage(docType);
            noOfPages = GetNoOfPages(workObject);
            doc = new FixedDocument();
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
                case DocType.LeavingCert: GenerateLeavingCert(); break;
                case DocType.Statement: GenerateStatement(); break;
                case DocType.FeesPayment: GenerateReceipt(); break;
                case DocType.Transcript: GenerateTranscript(); break;
                case DocType.Transcript2: GenerateTranscript2(); break;
                case DocType.Balances: GenerateBalanceList(); break;
                case DocType.ClassList: GenerateClassList(); break;
                // case DocType.Voucher: GenerateVoucher(); break;
                case DocType.ClassMarkList: GenerateClassMarkList(); break;
                case DocType.AggregateResult: GenerateAggregateResult(); break;
            }
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
                case DocType.Voucher: resString = Helper.Properties.Resources.PaymentVoucher; break;
                case DocType.ClassMarkList: resString = Helper.Properties.Resources.ClassMarkList; break;
                case DocType.AggregateResult: resString = Helper.Properties.Resources.AggregateResult; break;
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
            if (workObject is PaymentVoucherModel)
                return DocType.Voucher;
            if (workObject is ClassExamResultModel)
                return DocType.ClassMarkList;
            if (workObject is AggregateResultModel)
                return DocType.AggregateResult;
            
            throw new ArgumentException();
        }

        private static int GetItemsPerPage(DocType docType)
        {
            switch (docType)
            {
                case DocType.Statement: return 22;
                case DocType.ClassMarkList: return 37;
                case DocType.ClassList: return 34;
                case DocType.Balances: return 34;
                default: return 31;
            }
        }

        private static int GetNoOfPages(object workObject)
        {
            int totalNoOfItems = 0;
            if (docType == DocType.LeavingCert)
                return 1;
            if (docType == DocType.FeesPayment)
                return 1;
            if (docType == DocType.Transcript)
                return 1;
            if (docType == DocType.Transcript2)
                return 1;
            if (docType == DocType.Voucher)
                return 1;
            if (docType == DocType.AggregateResult)
                return 1;
            if (workObject is SaleModel)
                totalNoOfItems = (workObject as SaleModel).SaleItems.Count;
            if (workObject is FeesStatementModel)
                totalNoOfItems = (workObject as FeesStatementModel).Transactions.Count;
            if (workObject is ClassFeesDefaultModel)
                totalNoOfItems = (workObject as ClassFeesDefaultModel).Entries.Count;
            if (workObject is ClassStudentListModel)
                totalNoOfItems = (workObject as ClassStudentListModel).Entries.Count;
            if (workObject is ClassExamResultModel)
                totalNoOfItems = (workObject as ClassExamResultModel).Entries.Rows.Count;

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

        private static void AddTextWithWrap(string text, string fontFamily, double width, double height, double fontSize, bool isBold, double rotateAngle,
           Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily(fontFamily);
            text1.FontSize = fontSize;
            text1.TextWrapping = TextWrapping.Wrap;
            text1.Width = width;
            text1.Height = height;
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
        private static void AddTRPointsPosition(int points, int pageNo)
        {
            AddText(points.ToString(), "Arial", 14, true, 0, Colors.Black, 570, 580, pageNo);
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
                AddTRPointsPosition(si.Points, pageNo);
                AddTRTotalMarks(si.TotalMarks,pageNo);
                AddTRMeanGrade(si.MeanGrade, pageNo);
                AddTRSubjectScores(si.Entries,pageNo);
            }
        }
        #endregion

        #region Transcript2
        private static void AddTR2Responsibilities(string responsibilities, int pageNo)
        {
            AddTextWithWrap(responsibilities, "Arial",200,40, 14, false, 0, Colors.Black, 30,670, pageNo);
        }
        private static void AddTR2Clubs(string clubs, int pageNo)
        {
            AddTextWithWrap(clubs, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 740, pageNo);
        }
        private static void AddTR2Boarding(string boarding, int pageNo)
        {
            AddTextWithWrap(boarding, "Arial", 200, 60, 14, false, 0, Colors.Black, 30, 810, pageNo);
        }
        private static void AddTR2ClassTR(string classTR, int pageNo)
        {
            AddTextWithWrap(classTR, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 940, pageNo);
        }
        private static void AddTR2ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524,30, 14, false, 0, Colors.Black, 250, 940, pageNo);
        }
        private static void AddTR2Principal(string principal, int pageNo)
        {
            AddTextWithWrap(principal, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private static void AddTR2PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 1010, pageNo);
        }
        private static void AddTR2Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 350, 1055, pageNo);
        }
        private static void AddTR2Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 120, 1055, pageNo);
        }
        private static void AddTR2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 100, 135, pageNo);
        }
        private static void AddTR2Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 255, 135, pageNo);
        }
        private static void AddTR2ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 630, 135, pageNo);
        }
        private static void AddTR2KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 173, pageNo);
        }
        
        private static void AddTR2ClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }
        private static void AddTR2TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddTR2OverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 650, 580, pageNo);
        }
        private static void AddTR2MeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private static void AddTR2ClustPoints(int point, int pageNo)
        {
            AddText(point.ToString(), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }
        
        private static void AddTR2SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Cat1Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 263, yPos, pageNo);
            AddText(item.Cat2Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 327, yPos, pageNo);
            AddText(item.ExamScore.ToString(), "Arial", fontsize, false, 0, Colors.Black, 390, yPos, pageNo);
            AddText((item.ExamScore+item.Cat1Score+item.Cat2Score)>0?((item.ExamScore+item.Cat1Score+item.Cat2Score)/3).ToString("N0"):
            "0", "Arial", fontsize, false, 0, Colors.Black, 476, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 535, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 625, yPos, pageNo);            
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private static void AddTR2SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR2SubjectScore(psi[i], i, pageNo);
        }

        private static double GetYHeight(string grade)
        {
            switch (grade)
            {
                case "A": return 188;
                case "A-": return 171;
                case "B+": return 155;
                case "B": return 140;
                case "B-": return 123;
                case "C+": return 107;
                case "C": return 92;
                case "C-": return 75;
                case "D+": return 59;
                case "D": return 44;
                case "D-": return 27;
                case "E": return 12;
            }
            throw new ArgumentOutOfRangeException("Invalid grade value: " + grade);
        }

        private static void TR2DrawGraph(string kcpeGrade,string cat1Grade,string cat2Grade,string examGrade,int pageNo)
        {
            
            Border bd1, bd2, bd3, bd4;
            bd1 = new Border();
            bd1.Width = 10;
            bd1.Height = GetYHeight(kcpeGrade);
            bd1.HorizontalAlignment = HorizontalAlignment.Left;
            bd1.VerticalAlignment = VerticalAlignment.Bottom;
            bd1.Background = new SolidColorBrush(Colors.Gray);
            bd1.Margin = new Thickness(285, 0, 0, 271);

            bd2 = new Border();
            bd2.Width = 10;
            bd2.Height = GetYHeight(cat1Grade);
            bd2.HorizontalAlignment = HorizontalAlignment.Left;
            bd2.VerticalAlignment = VerticalAlignment.Bottom;
            bd2.Background = new SolidColorBrush(Colors.Gray);
            bd2.Margin = new Thickness(330, 0, 0, 271);

            bd3 = new Border();
            bd3.Width = 10;
            bd3.Height = GetYHeight(cat2Grade);
            bd3.HorizontalAlignment = HorizontalAlignment.Left;
            bd3.VerticalAlignment = VerticalAlignment.Bottom;
            bd3.Background = new SolidColorBrush(Colors.Gray);
            bd3.Margin = new Thickness(380, 0, 0, 271);

            bd4 = new Border();
            bd4.Width = 10;
            bd4.Height = GetYHeight(examGrade);
            bd4.HorizontalAlignment = HorizontalAlignment.Left;
            bd4.VerticalAlignment = VerticalAlignment.Bottom;
            bd4.Background = new SolidColorBrush(Colors.Gray);
            bd4.Margin = new Thickness(428, 0, 0, 271);

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(bd1);
            g.Children.Add(bd2);
            g.Children.Add(bd3);
            g.Children.Add(bd4);
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
                AddTR2KCPEScore(si.KCPEScore, pageNo);
                AddTR2OverAllPosition(si.OverAllPosition, pageNo);
                AddTR2TotalMarks(si.TotalMarks, pageNo);
                AddTR2MeanGrade(si.MeanGrade, pageNo);
                AddTR2SubjectScores(si.Entries, pageNo);
                AddTR2Boarding(si.Boarding,pageNo);
                AddTR2Responsibilities(si.Responsibilities,pageNo);
                AddTR2Clubs(si.ClubsAndSport,pageNo);
                AddTR2Principal(si.Principal, pageNo);
                AddTR2PrincipalComments(si.PrincipalComments, pageNo);
                AddTR2ClassTR(si.ClassTeacher, pageNo);
                AddTR2Opening(si.OpeningDay, pageNo);
                AddTR2Closing(si.ClosingDay, pageNo);
                AddTR2ClustPoints(si.Points, pageNo);
                AddTR2ClassTRComments(si.ClassTeacherComments, pageNo);
                TR2DrawGraph(DataAccess.CalculateGrade(si.KCPEScore/5),
                    DataAccess.CalculateGrade(si.Entries.Count > 0 ? (si.CAT1Score / si.Entries.Count) : 0),
                    DataAccess.CalculateGrade(si.Entries.Count>0?(si.CAT2Score / si.Entries.Count):0),
                    DataAccess.CalculateGrade(si.Entries.Count > 0 ? (si.ExamScore / si.Entries.Count) : 0), pageNo);
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
        /*
        #region Payment Voucher
        private static void AddPVPayee(string payee, int pageNo)
        {
            AddTextWithWrap(responsibilities, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 670, pageNo);
        }
        private static void AddPVVoucherNo(string clubs, int pageNo)
        {
            AddTextWithWrap(clubs, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 740, pageNo);
        }
        private static void AddPVAddress(string boarding, int pageNo)
        {
            AddTextWithWrap(boarding, "Arial", 200, 60, 14, false, 0, Colors.Black, 30, 810, pageNo);
        }
        private static void AddTR2ClassTR(string classTR, int pageNo)
        {
            AddTextWithWrap(classTR, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 940, pageNo);
        }
        private static void AddTR2ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 940, pageNo);
        }
        private static void AddTR2Principal(string principal, int pageNo)
        {
            AddTextWithWrap(principal, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private static void AddTR2PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 1010, pageNo);
        }
        private static void AddTR2Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 350, 1055, pageNo);
        }
        private static void AddTR2Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 120, 1055, pageNo);
        }
        private static void AddTR2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 100, 135, pageNo);
        }
        private static void AddTR2Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 255, 135, pageNo);
        }
        private static void AddTR2ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 630, 135, pageNo);
        }
        private static void AddTR2KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 173, pageNo);
        }

        private static void AddTR2ClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }
        private static void AddTR2TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString(), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddTR2OverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 650, 580, pageNo);
        }
        private static void AddTR2MeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private static void AddTR2ClustPoints(int point, int pageNo)
        {
            AddText(point.ToString(), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }

        private static void AddTR2SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Cat1Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 303, yPos, pageNo);
            AddText(item.Cat2Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 370, yPos, pageNo);
            AddText(item.ExamScore.ToString(), "Arial", fontsize, false, 0, Colors.Black, 440, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 535, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 625, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private static void AddTR2SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR2SubjectScore(psi[i], i, pageNo);
        }

        private static double GetYHeight(string grade)
        {
            switch (grade)
            {
                case "A": return 188;
                case "A-": return 171;
                case "B+": return 155;
                case "B": return 140;
                case "B-": return 123;
                case "C+": return 107;
                case "C": return 92;
                case "C-": return 75;
                case "D+": return 59;
                case "D": return 44;
                case "D-": return 27;
                case "E": return 12;
            }
            throw new ArgumentOutOfRangeException("Invalid grade value: " + grade);
        }

        private static void TR2DrawGraph(string kcpeGrade, string cat1Grade, string cat2Grade, string examGrade, int pageNo)
        {

            Border bd1, bd2, bd3, bd4;
            bd1 = new Border();
            bd1.Width = 10;
            bd1.Height = GetYHeight(kcpeGrade);
            bd1.HorizontalAlignment = HorizontalAlignment.Left;
            bd1.VerticalAlignment = VerticalAlignment.Bottom;
            bd1.Background = new SolidColorBrush(Colors.Gray);
            bd1.Margin = new Thickness(285, 0, 0, 271);

            bd2 = new Border();
            bd2.Width = 10;
            bd2.Height = GetYHeight(cat1Grade);
            bd2.HorizontalAlignment = HorizontalAlignment.Left;
            bd2.VerticalAlignment = VerticalAlignment.Bottom;
            bd2.Background = new SolidColorBrush(Colors.Gray);
            bd2.Margin = new Thickness(330, 0, 0, 271);

            bd3 = new Border();
            bd3.Width = 10;
            bd3.Height = GetYHeight(cat2Grade);
            bd3.HorizontalAlignment = HorizontalAlignment.Left;
            bd3.VerticalAlignment = VerticalAlignment.Bottom;
            bd3.Background = new SolidColorBrush(Colors.Gray);
            bd3.Margin = new Thickness(380, 0, 0, 271);

            bd4 = new Border();
            bd4.Width = 10;
            bd4.Height = GetYHeight(examGrade);
            bd4.HorizontalAlignment = HorizontalAlignment.Left;
            bd4.VerticalAlignment = VerticalAlignment.Bottom;
            bd4.Background = new SolidColorBrush(Colors.Gray);
            bd4.Margin = new Thickness(428, 0, 0, 271);

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(bd1);
            g.Children.Add(bd2);
            g.Children.Add(bd3);
            g.Children.Add(bd4);
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
                AddTR2KCPEScore(si.KCPEScore, pageNo);
                AddTR2OverAllPosition(si.OverAllPosition, pageNo);
                AddTR2TotalMarks(si.TotalMarks, pageNo);
                AddTR2MeanGrade(si.MeanGrade, pageNo);
                AddTR2SubjectScores(si.Entries, pageNo);
                AddTR2Boarding(si.Boarding, pageNo);
                AddTR2Responsibilities(si.Responsibilities, pageNo);
                AddTR2Clubs(si.ClubsAndSport, pageNo);
                AddTR2Principal(si.Principal, pageNo);
                AddTR2PrincipalComments(si.PrincipalComments, pageNo);
                AddTR2ClassTR(si.ClassTeacher, pageNo);
                AddTR2Opening(si.OpeningDay, pageNo);
                AddTR2Closing(si.ClosingDay, pageNo);
                AddTR2ClustPoints(si.Points, pageNo);
                AddTR2ClassTRComments(si.ClassTeacherComments, pageNo);
                TR2DrawGraph("A", "B", "C-", "D", pageNo);
            }
        }
        #endregion
        */

        #region Class Mark List

        private static void AddCMLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 70, 70, pageNo);
        }
        private static void AddCMLSubjects(DataTable entries, int pageNo)
        {
            double xPos = 255;
            double count = 0;
            
            for (int i = 2; i < entries.Columns.Count - 2;i++ )
            {
                AddText(entries.Columns[i].ColumnName.ToUpper().Substring(0, 3), "Times New Roman", 14, true, 0, Colors.Black, xPos + count * 35, 110, pageNo);
                count++;
            }
        }
        private static void AddCMLStudent(DataRow item, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 140 + pageRelativeIndex * 26;
            double xPos = 255;
            double count = 0;
            AddText(item["Student ID"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 16, yPos, pageNo);
            AddText(item["Name"].ToString().Length > 25 ? item["Name"].ToString().Substring(0, 25) : item["Name"].ToString()
                , "Times New Roman", 12, false, 0, Colors.Black, 50, yPos, pageNo);
            for(int i=2;i< item.ItemArray.Length-2;i++)
            {
                AddText(item[i].ToString(), "Times New Roman", 12, false, 0, Colors.Black, xPos + count * 35, yPos, pageNo);
                count++;
            }
            AddText(item["Total"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 680, yPos, pageNo);
            AddText(item["Position"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 745, yPos, pageNo);
        }
        private static void AddCMLStudents(DataTable psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Rows.Count)
                return;
            if (endIndex >= psi.Rows.Count)
                endIndex = psi.Rows.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCMLStudent(psi.Rows[i], i, pageNo);
        }

        private static void GenerateClassMarkList()
        {
            ClassExamResultModel si = myWorkObject as ClassExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddCMLClass(si.NameOfClass, pageNo);
                AddCMLSubjects(si.Entries, pageNo);
                AddCMLStudents(si.Entries, pageNo);
            }
        }
        #endregion

        #region Aggregate Result
        
        private static void AddAGClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private static void AddAGDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 650, 65, pageNo);
        }
        private static void AddAGExam(string nameOfExam, int pageNo)
        {
            AddText(nameOfExam, "Arial", 14, true, 0, Colors.Black, 100, 120, pageNo);
        }
        private static void AddAGMeanScore(decimal meanScore, int pageNo)
        {
            AddText(meanScore.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 155, 165, pageNo);
        }
        private static void AddAGMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 160, 210, pageNo);
        }
        private static void AddAGPoints(int points, int pageNo)
        {
            AddText(points.ToString(), "Arial", 14, true, 0, Colors.Black, 110, 255, pageNo);
        }
        private static void AddAGEntry(AggregateResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 355 + pageRelativeIndex * 25;

            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.MeanScore.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, 285, yPos, pageNo);
            AddText(item.MeanGrade, "Arial", fontsize, false, 0, Colors.Black, 475, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 630, yPos, pageNo);
        }
        private static void AddAGEntries(ObservableCollection<AggregateResultEntryModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddAGEntry(psi[i], i, pageNo);
        }

        private static void GenerateAggregateResult()
        {
            AggregateResultModel si = myWorkObject as AggregateResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAGClass(si.NameOfClass, pageNo);
                AddAGDate(DateTime.Now, pageNo);
                AddAGExam(si.NameOfExam, pageNo);
                AddAGMeanScore(si.MeanScore, pageNo);
                AddAGMeanGrade(si.MeanGrade, pageNo);
                AddAGPoints(si.Points, pageNo);
                AddAGEntries(si.Entries, pageNo);
            }
        }
        #endregion

    }

}
