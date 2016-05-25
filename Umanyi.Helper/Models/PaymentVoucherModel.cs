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
        private string description;
        private DateTime datePaid;

        public PaymentVoucherModel()
        {
            Description = "";
            PaymentVoucherID = 0;
            NameOfPayee = "";
            entries = new ObservableCollection<PaymentVoucherEntryModel>();
            Total = 0;
            Description = "";
            DatePaid = DateTime.Now;
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

        public DateTime DatePaid
        {
            get { return datePaid; }

            set
            {
                if (value != datePaid)
                {
                    datePaid = value;
                    NotifyPropertyChanged("DatePaid");
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

        public string Description
        {
            get { return description; }

            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public override void Reset()
        {
            Description = "";
            PaymentVoucherID = 0;
            NameOfPayee = "";
            Address = "";
            entries.Clear();
            Total = 0;
        }

        public string Category { get; set; }
    }
}
