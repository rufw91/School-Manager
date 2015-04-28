using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class FeePaymentReceipt2Model:FeePaymentReceiptModel
    {
        public FeePaymentReceipt2Model()
        {
        }
        public FeePaymentReceipt2Model(FeePaymentReceiptModel baseItem)
        {
            CopyFrom(baseItem);
        }
        private void CopyFrom(FeePaymentReceiptModel baseItem)
        {
            this.AmountPaid = baseItem.AmountPaid;
            this.DatePaid = baseItem.DatePaid;
            this.Entries = baseItem.Entries;
            this.FeePaymentID = baseItem.FeePaymentID;
            this.NameOfClass = baseItem.NameOfClass;
            this.NameOfStudent = baseItem.NameOfStudent;
            this.StudentID = baseItem.StudentID;
        }
    }
}
