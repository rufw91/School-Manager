using System;
using System.Collections.Generic;
using UmanyiSMS.Modules.Staff.Controller;
namespace UmanyiSMS.Modules.Staff.Models
{
    public class StaffSelectModel : StaffBaseModel
    {
        private bool isActive;
        public StaffSelectModel()
        {
            this.IsActive = true;
            CheckErrors();
        }

        public bool IsActive
        {
            get { return this.isActive; }

            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
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
                StaffModel staff = DataController.GetStaff(StaffID);
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
    }
}
