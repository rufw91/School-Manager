using System;

namespace Helper.Models
{
    public class DonorListModel : DonorModel
    {
        private decimal totalDonations;

        public decimal TotalDonations
        {
            get
            {
                return this.totalDonations;
            }
            set
            {
                if (value != this.totalDonations)
                {
                    this.totalDonations = value;
                    base.NotifyPropertyChanged("TotalDonations");
                }
            }
        }

        public DonorListModel()
        {
            this.TotalDonations = 0m;
        }

        public override void Reset()
        {
            base.Reset();
            this.TotalDonations = 0m;
        }
    }
}
