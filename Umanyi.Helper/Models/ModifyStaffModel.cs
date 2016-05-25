using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ModifyStaffModel: StaffModel
    {
        public override bool CheckErrors()
        {
            try
            {
                ClearAllErrors();
                if (StaffID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Staff member does not exist.");
                    SetErrors("StaffID", errors);
                    Clean();
                }
                else
                {
                    StaffModel staff = DataAccess.GetStaff(StaffID);
                    if (staff.StaffID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Staff member does not exist.");
                        SetErrors("StaffID", errors);
                        Clean();
                    }
                    else
                    {
                        ClearErrors("StaffID");
                        this.Name = staff.Name;
                        this.Address = staff.Address;
                        this.City = staff.City;
                        this.DateOfAdmission = staff.DateOfAdmission;
                        this.Email = staff.Email;
                        this.NationalID = staff.NationalID;
                        this.PhoneNo = staff.PhoneNo;
                        this.PostalCode = staff.PostalCode;
                        this.SPhoto = staff.SPhoto;
                        this.Designation = staff.Designation;   
                    }
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }

        private void Clean()
        {
            Name = "";
            DateOfAdmission = new DateTime(1900, 1, 1);
            NationalID = "";
            PhoneNo = "";
            Email = "";
            Address = "";
            City = "";
            PostalCode = "";
            SPhoto = null;
        }
    }
}
