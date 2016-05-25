using Helper.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region Receipt

        
        private static void AddPSDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 380, 100, pageNo);
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 380 + page2Offset, 100, pageNo);
        }
        private static void AddPSPaymentPeriod(string paymentPeriod, int pageNo)
        {
            AddText(paymentPeriod, 14, true, 0, Colors.Black, 145, 145, pageNo);
            AddText(paymentPeriod, 14, true, 0, Colors.Black, 145 + page2Offset, 145, pageNo);
        }
        private static void AddPSStaffName(string staffName, int pageNo)
        {
            AddText(staffName.ToUpperInvariant(), 14, true, 0, Colors.Black, 145, 205, pageNo);
            AddText(staffName.ToUpperInvariant(), 14, true, 0, Colors.Black, 145 + page2Offset, 205, pageNo);
        }
        private static void AddPSDesignation(string designation, int pageNo)
        {
            AddText(designation, 14, true, 0, Colors.Black, 145, 235, pageNo);
            AddText(designation, 14, true, 0, Colors.Black, 145 + page2Offset, 235, pageNo);
        }
        private static void AddPSBasicPay(decimal basicPay, int pageNo)
        {
            AddText(basicPay.ToString("N2"), 14, true, 0, Colors.Black, 145, 265, pageNo);
            AddText(basicPay.ToString("N2"), 14, true, 0, Colors.Black, 145 + page2Offset, 265, pageNo);
        }

        private static void AddPSEntry(FeesStructureEntryModel item, int itemIndex, int pageNo, bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 320 + pageRelativeIndex * 30;

            if (isAggregate)
            {
                AddText(item.Name, 16, true, 0, Colors.Black, 50, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 280, yPos, pageNo);
                AddText(item.Name, 16, true, 0, Colors.Black, 50 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 280 + page2Offset, yPos, pageNo);
            }
            else
            {
                AddText(item.Name, fontsize, false, 0, Colors.Black, 50, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 280, yPos, pageNo);
                AddText(item.Name, fontsize, false, 0, Colors.Black, 50 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 280 + page2Offset, yPos, pageNo);
            }
        }
        private static void AddPSEntries(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddPSEntry(psi[i], i, pageNo, (i == endIndex));

        }

        private static void GeneratePayslip()
        {
            PayslipModel si = myWorkObject as PayslipModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddPSDate(DateTime.Now, pageNo);
                AddPSPaymentPeriod(si.PaymentPeriod, pageNo);
                AddPSStaffName(si.Name, pageNo);
                AddPSDesignation(si.Designation, pageNo);
                AddPSBasicPay(si.AmountPaid, pageNo);
                AddPSEntries(si.Entries, pageNo);
            }
        }


        #endregion
    }
}
