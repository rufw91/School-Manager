using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class RemoveBillVM: ViewModelBase
    {
        StudentSelectModel selectedStudent;
        private SaleModel currentBill;
        private decimal billTotal;
        public RemoveBillVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "REMOVE BILL";
            currentBill = new SaleModel();
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    currentBill.SaleItems.Clear();
                    SaleModel s = await DataAccess.GetThisTermInvoice(selectedStudent.StudentID);
                    foreach (var f in s.SaleItems)
                        currentBill.SaleItems.Add(f);
                    currentBill.SaleID = s.SaleID;
                }
            };
            currentBill.SaleItems.CollectionChanged += (o, e) =>
            {
                BillTotal = 0;
                foreach (var v in currentBill.SaleItems)
                    BillTotal += v.Amount;
            };
        }

        protected override void CreateCommands()
        {
            DeleteCommand = new RelayCommand(async o =>
            {
                currentBill.RefreshTotal();
                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you would like to delete this Bill: \r\nStudent ID:"
                    + selectedStudent.StudentID + "\r\nAmount:" + currentBill.OrderTotal, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool succ = await DataAccess.RemoveSaleAsync(currentBill.SaleID);
                    MessageBox.Show("Succesfully completed operation.");
                    Reset();
                }

            }, o => CanDelete());
        }

        private bool CanDelete()
        {
            selectedStudent.CheckErrors();
            
            return !selectedStudent.HasErrors &&
            (currentBill != null) && (currentBill.SaleID > 0);
        }
        public decimal BillTotal
        {
            get { return billTotal; }

            private set
            {
                if (value != billTotal)
                {
                    billTotal = value;
                    NotifyPropertyChanged("BillTotal");
                }
            }
        }
        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if (value != selectedStudent)
                {
                    selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }
        public SaleModel CurrentBill
        {
            get { return currentBill; }
            set
            {
                if (value != currentBill)
                {
                    currentBill = value;
                    NotifyPropertyChanged("CurrentBill");
                }
            }
        }
        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            selectedStudent.Reset();
            currentBill.Reset();
        }
    }
}
