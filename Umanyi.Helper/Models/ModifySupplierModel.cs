﻿using System;
using System.Collections.Generic;

namespace Helper.Models
{
    public class ModifySupplierModel: SupplierModel
    {
        public override bool CheckErrors()
        {
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Incomplete;
            try
            {
                ClearAllErrors();
                if (SupplierID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Supplier does not exist.");
                    SetErrors("SupplierID", errors);
                }
                else
                {
                    SupplierModel supplier = DataAccess.GetSupplier(SupplierID);
                    if (supplier.SupplierID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Supplier does not exist.");
                        SetErrors("SupplierID", errors);
                    }
                    else
                    {
                        ClearErrors("SupplierID");
                        this.SupplierID = supplier.SupplierID;
                        this.Address = supplier.Address;
                        this.AltPhoneNo = supplier.AltPhoneNo;
                        this.City = supplier.City;
                        this.Email = supplier.Email;
                        this.NameOfSupplier = supplier.NameOfSupplier;
                        this.PhoneNo = supplier.PhoneNo;
                        this.PINNo = supplier.PINNo;
                        this.PostalCode = supplier.PostalCode;                        
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
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Complete;
            return HasErrors;
        }
    }
}