using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class PaymentVoucherEntryModel: ModelBase
    {
        public PaymentVoucherEntryModel()
        {
            Description = "";
            Amount = 0;
            DatePaid = DateTime.Now;
        }
        public string Description { get; set; }
        public DateTime DatePaid { get; set; }
        public decimal Amount { get; set; }

        public override void Reset()
        {
            Description = "";
            DatePaid = DateTime.Now;
            Amount = 0;
        }

    }
}
