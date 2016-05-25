using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Helper.Models
{
    public class PayslipModel : StaffSelectModel
    {
        private ObservableCollection<FeesStructureEntryModel> entries;
        private string designation;

        public ObservableCollection<FeesStructureEntryModel> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

        public decimal AmountPaid
        {
            get;
            set;
        }

        public DateTime DatePaid
        {
            get;
            set;
        }

        public string Designation
        {
            get
            {
                return this.designation;
            }
            set
            {
                if (value != this.designation)
                {
                    this.designation = value;
                    base.NotifyPropertyChanged("Designation");
                }
            }
        }

        public int PayslipID
        {
            get;
            set;
        }

        public PayslipModel()
        {
            this.Entries = new ObservableCollection<FeesStructureEntryModel>();
            this.DatePaid = DateTime.Now;
            AmountPaid = 0; 
            this.Designation = "";
            PaymentPeriod = "";
        }
        public override bool CheckErrors()
        {
            ClearAllErrors();
            if (StaffID == 0)
            {
                List<string> errors = new List<string>();
                errors.Add("Staff member does not exist.");
                SetErrors("StaffID", errors);
                Name = "";
                this.IsActive = true;
            }
            else
            {
                StaffModel staff = DataAccess.GetStaff(StaffID);
                if (staff.StaffID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Staff member does not exist.");
                    SetErrors("StaffID", errors);
                    Name = "";
                }
                else
                {
                    ClearErrors("StaffID");
                    this.StaffID = staff.StaffID;
                    this.Name = staff.Name;
                    this.Designation = staff.Designation;
                    this.IsActive = staff.IsActive;
                    /*if (!this.isActive)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("StaffID member is not active.");
                        SetErrors("StaffID", errors);
                    }*/

                }
            }

            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }

        public void RefreshTotal()
        {
            decimal num = 0m;
            foreach (FeesStructureEntryModel current in this.entries)
            {
                num += current.Amount;
            }
            num = this.AmountPaid - num;
            if (!this.entries.Any((FeesStructureEntryModel o) => o.Name == "TOTAL"))
            {
                this.Entries.Add(new FeesStructureEntryModel
                {
                    Name = "TOTAL",
                    Amount = num
                });
            }
            else
            {
                this.entries.First((FeesStructureEntryModel o) => o.Name == "TOTAL").Amount = num;
            }
        }

        public override void Reset()
        {
            base.Reset();
            Designation = "";
            AmountPaid = 0;
            PayslipID = 0;
            DatePaid = DateTime.Now;
            PaymentPeriod = "";
            entries.Clear();
        }

        public string PaymentPeriod { get; set; }
    }
}
