using System;
using UmanyiSMS;

namespace Helper.Models
{
    public class FeePaymentReceiptModel : FeePaymentModel
    {
        public string NameOfClass
        {
            get;
            set;
        }

        public ObservableImmutableList<FeesStructureEntryModel> Entries
        {
            get;
            set;
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}
