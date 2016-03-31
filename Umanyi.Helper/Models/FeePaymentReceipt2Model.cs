using System;

namespace Helper.Models
{
    public class FeePaymentReceipt2Model : FeePaymentReceiptModel
    {
        public FeePaymentReceipt2Model()
        {
        }

        public FeePaymentReceipt2Model(FeePaymentReceiptModel baseItem)
        {
            this.CopyFrom(baseItem);
        }

        private void CopyFrom(FeePaymentReceiptModel baseItem)
        {
            base.AmountPaid = baseItem.AmountPaid;
            base.DatePaid = baseItem.DatePaid;
            base.Entries = baseItem.Entries;
            base.FeePaymentID = baseItem.FeePaymentID;
            base.NameOfClass = baseItem.NameOfClass;
            base.NameOfStudent = baseItem.NameOfStudent;
            base.StudentID = baseItem.StudentID;
        }
    }
}
