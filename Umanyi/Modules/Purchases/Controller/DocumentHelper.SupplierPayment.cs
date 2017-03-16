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
          #region Payment Voucher2
          
          private static void AddPV2VoucherNo(int voucherNo, int pageNo)
          {
              AddText(voucherNo.ToString(), "Arial", 14, true, 0, Colors.Black, 665, 175, pageNo);
          }
          private static void AddPV2Payee(string payee, int pageNo)
          {
              AddText(payee.ToUpper(), "Arial", 14, true, 0, Colors.Black, 115, 220, pageNo);
          }
          private static void AddPV2Tel(string tel, int pageNo)
          {
              AddText(tel, "Arial", 14, true, 0, Colors.Black, 115, 250, pageNo);
          }

          private static void AddPV2Description(string description, int pageNo)
          {
              AddTextWithWrap(description, "Arial", 415, 375, 14, false, 0, Colors.Black, 90, 370, pageNo);
          }
          private static void AddPV2Amount(decimal amount, int pageNo)
          {
              AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 605, 400, pageNo);
          }

          private static void AddPV2AmtInWords(int amount, int pageNo)
          {
              //AddTextWithWrap(description, "Arial", 700, 53, 14, false, 0, Colors.Black, 30, 860, pageNo);
          }
          private static void GenerateVoucher2()
          {
              SupplierPaymentModel si = myWorkObject as SupplierPaymentModel;

              int pageNo;
              for (pageNo = 0; pageNo < noOfPages; pageNo++)
              {
                  AddPV2VoucherNo(si.SupplierPaymentID, pageNo);
                  AddPV2Payee(si.NameOfSupplier, pageNo);
                  AddPV2Description(si.Notes, pageNo);
                  AddPV2Amount(si.AmountPaid, pageNo);
                  AddPV2AmtInWords((int)si.AmountPaid, pageNo);
              }
          }
          #endregion
      }
}
