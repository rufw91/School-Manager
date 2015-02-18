using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class FeesStatementModel : StudentBaseModel
    {
        public FeesStatementModel()
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

        public override bool CheckErrors()
        {
            try
            {
                ClearAllErrors();
                if (StudentID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student does not exist.");
                    SetErrors("StudentID", errors);
                }
                else
                {
                    StudentModel student = DataAccess.GetStudent(StudentID);
                    if (student.StudentID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student does not exist.");
                        SetErrors("StudentID", errors);
                    }
                    else
                    {
                        ClearErrors("StudentID");
                        this.StudentID = student.StudentID;
                        this.NameOfStudent = student.NameOfStudent;
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