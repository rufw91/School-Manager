using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class RemoveBillVM: ViewModelBase
    {
        StudentSelectModel selectedStudent;
        private SaleModel currentBill;
        private decimal billTotal;
        private TermModel selectedTerm;
        ObservableCollection<TermModel> allTerms;
        public RemoveBillVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "REMOVE BILL";
            currentBill = new SaleModel();
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += OnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
            AllTerms = await DataAccess.GetAllTermsAsync();
            currentBill.SaleItems.CollectionChanged += (o, e) =>
            {
                BillTotal = 0;
                foreach (var v in currentBill.SaleItems)
                    BillTotal += v.Amount;
            };
            
        }

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StudentID" || e.PropertyName == "SelectedTerm")
            {
                currentBill.SaleItems.Clear();
                if (selectedTerm != null)
                {
                    SaleModel s = await DataAccess.GetTermInvoice(selectedStudent.StudentID, selectedTerm);
                    foreach (var f in s.SaleItems)
                        currentBill.SaleItems.Add(f);
                    currentBill.SaleID = s.SaleID;
                }
            }
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

        public ObservableCollection<TermModel> AllTerms
        {
            get { return this.allTerms; }

            private set
            {
                if (value != this.allTerms)
                {
                    this.allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
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
        public TermModel SelectedTerm
        {
            get { return selectedTerm; }
            set
            {
                if (value != selectedTerm)
                {
                    selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
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
