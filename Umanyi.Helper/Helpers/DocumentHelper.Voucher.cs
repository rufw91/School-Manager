using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        
      #region Payment Voucher
       private static void AddPVPayee(string payee, int pageNo)
       {
           AddText(payee, "Arial", 14, false, 0, Colors.Black, 130, 215, pageNo);
       }
       private static void AddPVVoucherNo(string voucherNo, int pageNo)
       {
           AddTextWithWrap(voucherNo, "Arial", 200, 40, 14, false, 0, Colors.Black, 650, 175, pageNo);
       }
       private static void AddPVAddress(string address, int pageNo)
       {
           AddTextWithWrap(address, "Arial", 200, 60, 14, false, 0, Colors.Black, 95, 245, pageNo);
       }
       private static void AddPVDescription(string description, int pageNo)
       {
           AddTextWithWrap(description, "Arial", 400, 160, 14, false, 0, Colors.Black, 135, 360, pageNo);
       }
       private static void AddPVAmount(decimal amount, int pageNo)
       {
           AddText(amount.ToString("N2"), "Arial", 14, false, 0, Colors.Black, 605, 350, pageNo);
       }
       
       private static void AddPVEntry(PaymentVoucherEntryModel item, int itemIndex, int pageNo)
       {
           double fontsize = 14;
           int pageRelativeIndex = itemIndex;
           double yPos = 832 + pageRelativeIndex * 30;

           AddText(item.Description, "Arial", 14, false, 0, Colors.Black, 230, yPos, pageNo);
           AddText(item.Amount.ToString("N2"), "Arial", 14, false, 0, Colors.Black, 615, yPos, pageNo);
           AddText("1", "Arial", fontsize, false, 0, Colors.Black, 95, yPos, pageNo);
       }
       private static void AddPVEntries(ObservableCollection<PaymentVoucherEntryModel> psi, int pageNo)
       {
           for (int i = 0; i <= psi.Count - 1; i++)
               AddPVEntry(psi[i], i, pageNo);
       }

      
       private static void GenerateVoucher()
       {
           PaymentVoucherModel  si = myWorkObject as PaymentVoucherModel;

           int pageNo;
           for (pageNo = 0; pageNo < noOfPages; pageNo++)
           {
               AddPVAddress(si.Address, pageNo);
               AddPVPayee(si.NameOfPayee, pageNo);
               AddPVVoucherNo("0", pageNo);
               AddPVDescription(si.Description, pageNo);
               AddPVAmount(si.Total,pageNo);
               AddPVEntries(si.Entries, pageNo);
               
           }
       }
       #endregion
    }
}
