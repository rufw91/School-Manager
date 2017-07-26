using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Projects.Models
{
    public class DonorModel : ModelBase
    {
        private int donorID;

        private string nameOfDonor;

        private string phoneNo;

        public int DonorID
        {
            get
            {
                return this.donorID;
            }
            set
            {
                if (value != this.donorID)
                {
                    this.donorID = value;
                    base.NotifyPropertyChanged("DonorID");
                }
            }
        }

        public string NameOfDonor
        {
            get
            {
                return this.nameOfDonor;
            }
            set
            {
                if (value != this.nameOfDonor)
                {
                    this.nameOfDonor = value;
                    base.NotifyPropertyChanged("NameOfDonor");
                }
            }
        }

        public string PhoneNo
        {
            get
            {
                return this.phoneNo;
            }
            set
            {
                if (value != this.phoneNo)
                {
                    this.phoneNo = value;
                    base.NotifyPropertyChanged("PhoneNo");
                }
            }
        }

        public DonorModel()
        {
            this.NameOfDonor = "";
            this.PhoneNo = "";
        }

        public override void Reset()
        {
            this.DonorID = 0;
            this.NameOfDonor = "";
            this.PhoneNo = "";
        }
    }
}
