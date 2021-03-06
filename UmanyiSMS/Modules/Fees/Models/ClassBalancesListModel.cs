﻿using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class ClassBalancesListModel : ClassModel
    {
        public DateTime Date
        {
            get;
            set;
        }

        public ObservableCollection<StudentFeesDefaultModel> Entries
        {
            get;
            set;
        }

        public decimal Total
        {
            get;
            set;
        }

        public decimal TotalUnpaid
        {
            get;
            set;
        }

        public ClassBalancesListModel()
        {
            this.Date = DateTime.Now;
            this.Entries = new ObservableCollection<StudentFeesDefaultModel>();
        }

        public override void Reset()
        {
        }
    }
}
