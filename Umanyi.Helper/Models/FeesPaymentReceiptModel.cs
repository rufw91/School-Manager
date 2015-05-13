using UmanyiSMS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Helper.Models
{
    public class FeePaymentReceiptModel : FeePaymentModel
    {
        public FeePaymentReceiptModel()
        { }



        public override void Reset()
        {
            base.Reset();
        }

        public string NameOfClass { get; set; }

        public ObservableImmutableList<FeesStructureEntryModel> Entries { get; set; }
    }
}
