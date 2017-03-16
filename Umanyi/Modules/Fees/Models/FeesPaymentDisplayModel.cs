using System;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class FeesPaymentDisplayModel : FeePaymentModel
    {
        public FeesPaymentDisplayModel()
        {
            this.CheckErrors();
        }

        public FeesPaymentDisplayModel(FeePaymentModel parent)
        {
            this.CheckErrors();
            base.AmountPaid = parent.AmountPaid;
            base.DatePaid = parent.DatePaid;
            base.FeePaymentID = parent.FeePaymentID;
            base.NameOfStudent = parent.NameOfStudent;
            base.StudentID = parent.StudentID;
            base.PaymentMethod = parent.PaymentMethod;
            this.CheckErrors();
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            base.NotifyPropertyChanged("HasErrors");
            return false;
        }
    }
}
