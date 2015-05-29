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
    public static partial class DocumentHelper
    {
        static FixedDocument doc;
        static DocType docType;
        static int noOfPages;
        static int itemsPerPage;
        static object myWorkObject;
        enum DocType
        {
            Statement, Transcript, LeavingCert, FeesPayment, FeesPayment2, Balances, ClassList,ClassExamResults,ClassLeavingCertificates,Transcript2, Voucher, ClassMarkList, AggregateResult, ClassTranscripts, UnreturnedBooks, Report
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
                case DocType.ClassLeavingCertificates: GenerateClassLeavingCerts(); break;
                case DocType.Statement: GenerateStatement(); break;
                case DocType.FeesPayment: GenerateReceipt(); break;
                case DocType.FeesPayment2: GenerateReceipt2(); break;
                case DocType.Transcript: GenerateTranscript(); break;
                case DocType.Transcript2: GenerateTranscript2(); break;
                case DocType.Balances: GenerateBalanceList(); break;
                case DocType.ClassList: GenerateClassList(); break;
                case DocType.Voucher: GenerateVoucher(); break;
                case DocType.ClassMarkList: GenerateClassMarkList(); break;
                case DocType.AggregateResult: GenerateAggregateResult(); break;
                case DocType.ClassTranscripts: GenerateClassTranscripts(); break;
                case DocType.UnreturnedBooks: GenerateUnreturnedBooks(); break;
                case DocType.Report: GenerateReport(); break;
                case DocType.ClassExamResults: GenerateClassExamResults(); break;
            }
        }

        private static FixedPage GetPage(DocType docType)
        {
            string resString = "";
            switch (docType)
            {
                case DocType.Statement: resString = Helper.Properties.Resources.Statement; break;
                case DocType.LeavingCert: resString = Helper.Properties.Resources.LeavingCert; break;
                case DocType.ClassLeavingCertificates: resString = Helper.Properties.Resources.LeavingCert; break;
                case DocType.FeesPayment: resString = Helper.Properties.Resources.Receipt; break;
                case DocType.FeesPayment2: resString = Helper.Properties.Resources.Receipt2; break;
                case DocType.Transcript: resString = Helper.Properties.Resources.Transcript; break;
                case DocType.Balances: resString = Helper.Properties.Resources.Balances; break;
                case DocType.ClassList: resString = Helper.Properties.Resources.ClassList; break;
                case DocType.Transcript2: resString = Helper.Properties.Resources.Transcript2; break;
                case DocType.ClassTranscripts: resString = Helper.Properties.Resources.Transcript2; break;
                case DocType.ClassExamResults: resString = Helper.Properties.Resources.Transcript; break;
                case DocType.Voucher: resString = Helper.Properties.Resources.PaymentVoucher; break;
                case DocType.ClassMarkList: resString = Helper.Properties.Resources.ClassMarkList; break;
                case DocType.AggregateResult: resString = Helper.Properties.Resources.AggregateResult; break;
                case DocType.UnreturnedBooks: resString = Helper.Properties.Resources.UnreturnedBooks; break;
                case DocType.Report: resString = Helper.Properties.Resources.Report; break;
                default: throw new ArgumentException();
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
            if (workObject is FeePaymentReceipt2Model)
                return DocType.FeesPayment2;
            if (workObject is FeePaymentReceiptModel)
                return DocType.FeesPayment;
            if (workObject is ClassBalancesListModel)
                return DocType.Balances;
            if (workObject is ClassStudentListModel)
                return DocType.ClassList;
            if (workObject is PaymentVoucherModel)
                return DocType.Voucher;
            if (workObject is ClassExamResultModel)
                return DocType.ClassMarkList;
            if (workObject is AggregateResultModel)
                return DocType.AggregateResult;
            if (workObject is UnreturnedBooksModel)
                return DocType.UnreturnedBooks;
            if (workObject is ClassTranscriptsModel)
                return DocType.ClassTranscripts;
            if (workObject is ReportModel)
                return DocType.Report;
            if (workObject is ClassStudentsExamResultModel)
                return DocType.ClassExamResults;
            if (workObject is ClassLeavingCertificatesModel)
                return DocType.ClassLeavingCertificates;
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
                case DocType.UnreturnedBooks: return 34;
                case DocType.Report: return 37;
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
            if (docType == DocType.FeesPayment2)
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
            if (workObject is ClassBalancesListModel)
                totalNoOfItems = (workObject as ClassBalancesListModel).Entries.Count;
            if (workObject is ClassStudentListModel)
                totalNoOfItems = (workObject as ClassStudentListModel).Entries.Count;
            if (workObject is ClassExamResultModel)
                totalNoOfItems = (workObject as ClassExamResultModel).Entries.Rows.Count;
            if (workObject is UnreturnedBooksModel)
                totalNoOfItems = (workObject as UnreturnedBooksModel).Entries.Count;            
            if (workObject is ClassTranscriptsModel)
                return (workObject as ClassTranscriptsModel).Entries.Count;
            if (workObject is ClassStudentsExamResultModel)
                return (workObject as ClassStudentsExamResultModel).Entries.Count;
            if (workObject is ClassLeavingCertificatesModel)
                return (workObject as ClassLeavingCertificatesModel).Entries.Count;
            if (workObject is ReportModel)
                totalNoOfItems = (workObject as ReportModel).Entries.Rows.Count;
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

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);

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

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);

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

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);
            

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }

        private static void AddText(string text, double left, double top, int pageNo)
        {
            AddText(text, 12, false, 0, Colors.Black, left, top, pageNo);
        }

        private static string GetTerm()
        {
            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 4)
                return "TERM I";
            else
                if (DateTime.Now.Month >= 5 && DateTime.Now.Month <= 8)
                    return "TERM II";
                else
                    return "TERM III";
        }
    }

}
