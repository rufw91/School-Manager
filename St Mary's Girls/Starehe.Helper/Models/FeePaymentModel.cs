using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class FeePaymentModel : StudentBaseModel
    {
        int feePaymentID;
        decimal amtPaid = 0;
        DateTime datePaid = DateTime.Now;
        public FeePaymentModel()
            : base()
        {
        }

        public DateTime DatePaid
        {
            get { return this.datePaid; }

            set
            {
                if (value != this.datePaid)
                {
                    this.datePaid = value;
                    NotifyPropertyChanged("DatePaid");
                }
            }
        }
        public int FeePaymentID
        {
            get { return this.feePaymentID; }

            set
            {
                if (value != this.feePaymentID)
                {
                    this.feePaymentID = value;
                    NotifyPropertyChanged("FeePaymentID");
                }
            }
        }
        public decimal AmountPaid
        {
            get { return this.amtPaid; }

            set
            {
                if (value != this.amtPaid)
                {
                    this.amtPaid = value;
                    NotifyPropertyChanged("AmountPaid");
                }
            }
        }
        
        public override bool CheckErrors()
        {
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Incomplete;
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
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Complete;
            return HasErrors;
        }
        public override void Reset()
        {
            base.Reset();
            DatePaid = DateTime.Now;
            AmountPaid = 0;
            FeePaymentID = 0;
        }

    }
}
