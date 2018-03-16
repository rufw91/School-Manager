using System;
using UmanyiSMS;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Fees.Models
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
