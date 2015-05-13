using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class PaymentVoucherModel:ModelBase
    {
        private int paymentVoucherID;
        private string nameOfPayee;
        private string address;
        private ObservableCollection<PaymentVoucherEntryModel> entries;
        private decimal total;

        public PaymentVoucherModel()
        {
            PaymentVoucherID = 0;
            NameOfPayee = "";
            entries = new ObservableCollection<PaymentVoucherEntryModel>();
            Total = 0;
            entries.CollectionChanged += (o, e) =>
                {
                    Total = 0;
                    foreach (var f in entries)
                        Total += f.Amount;
                };
        }

        public int PaymentVoucherID
        {
            get { return paymentVoucherID; }

            set
            {
                if (value != paymentVoucherID)
                {
                    paymentVoucherID = value;
                    NotifyPropertyChanged("PaymentVoucherID");
                }
            }
        }

        public string NameOfPayee
        {
            get { return nameOfPayee; }

            set
            {
                if (value != nameOfPayee)
                {
                    nameOfPayee = value;
                    NotifyPropertyChanged("NameOfPayee");
                }
            }
        }

        public string Address
        {
            get { return address; }

            set
            {
                if (value != address)
                {
                    address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        public decimal Total
        {
            get { return total; }

            set
            {
                if (value != total)
                {
                    total = value;
                    NotifyPropertyChanged("Total");
                }
            }
        }

        public ObservableCollection<PaymentVoucherEntryModel> Entries
        {
            get { return entries; }

            set
            {
                if (value != entries)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public override void Reset()
        {
            PaymentVoucherID = 0;
            NameOfPayee = "";
            Address = "";
            entries.Clear();
            Total = 0;
        }
    }
}
