using Helper.Presentation;
using System;
using System.Collections.Generic;

namespace Helper.Models
{
    public class DonationModel : DonorModel
    {
        private DonateTo donateTo;

        private decimal amount;

        private DateTime dateDonated;

        private int donationID;

        public int DonationID
        {
            get
            {
                return this.donationID;
            }
            set
            {
                if (value != this.donationID)
                {
                    this.donationID = value;
                    base.NotifyPropertyChanged("DonationID");
                }
            }
        }

        public DonateTo DonateTo
        {
            get
            {
                return this.donateTo;
            }
            set
            {
                if (value != this.donateTo)
                {
                    this.donateTo = value;
                    base.NotifyPropertyChanged("DonateTo");
                }
            }
        }

        public decimal Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (value != this.amount)
                {
                    this.amount = value;
                    base.NotifyPropertyChanged("Amount");
                }
            }
        }

        public DateTime DateDonated
        {
            get
            {
                return this.dateDonated;
            }
            set
            {
                if (value != this.dateDonated)
                {
                    this.dateDonated = value;
                    base.NotifyPropertyChanged("DateDonated");
                }
            }
        }

        public DonationModel()
        {
            this.DonateTo = DonateTo.Fees;
            this.DateDonated = DateTime.Now;
        }

        public override void Reset()
        {
            base.Reset();
            this.Amount = 0m;
            this.DonateTo = DonateTo.Fees;
            this.DateDonated = DateTime.Now;
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            if (base.DonorID == 0)
            {
                base.SetErrors("DonorID", new List<string>
                {
                    "Donor does not exist."
                });
                base.NameOfDonor = "";
            }
            else
            {
                DonorModel donor = DataAccess.GetDonor(base.DonorID);
                if (donor.DonorID == 0)
                {
                    base.SetErrors("DonorID", new List<string>
                    {
                        "Donor does not exist."
                    });
                    base.NameOfDonor = "";
                }
                else
                {
                    base.ClearErrors("DonorID");
                    base.DonorID = donor.DonorID;
                    base.NameOfDonor = donor.NameOfDonor;
                }
            }
            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }
    }
}
