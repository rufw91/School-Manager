﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class SupplierStatementModel: SupplierBaseModel
    {
        public SupplierStatementModel()
        {
            Transactions = new ObservableCollection<TransactionModel>();
            StatementID = "";
            DateOfStatement = DateTime.Now;
            TotalDue = 0;
            TotalPayments = 0;
            TotalSales = 0;
            From = DateTime.Now;
            To = DateTime.Now;
        }
        public decimal BalanceBroughtForward { get; set; }
        public ObservableCollection<TransactionModel> Transactions
        {
            get;
            set;
        }
        public string Period
        {
            get { return From.ToString("dd MMM yyyy") + " to " + To.ToString("dd MMM yyyy"); }
        }
        public string StatementID
        {
            get;
            set;
        }
        public DateTime DateOfStatement
        {
            get;
            set;
        }
        public decimal TotalDue
        {
            get;
            set;
        }

        public decimal TotalPayments
        {
            get;
            set;
        }

        public decimal TotalSales
        {
            get;
            set;
        }

        public DateTime To
        {
            get;
            set;
        }

        public DateTime From
        {
            get;
            set;
        }

        public override void Reset()
        {
            base.Reset();
            Transactions = new ObservableCollection<TransactionModel>();
            StatementID = "";
            DateOfStatement = DateTime.Now;
            TotalDue = 0;
            TotalPayments = 0;
            TotalSales = 0;
            BalanceBroughtForward = 0;
            From = DateTime.Now;
            To = DateTime.Now;
        }
    }
}