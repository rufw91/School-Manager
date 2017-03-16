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
    public class RemovePaymentVM:ViewModelBase
    {
        ObservableCollection<FeePaymentModel> recentPayments;
        StudentSelectModel selectedStudent;
        FeePaymentModel selectedPayment;
        public RemovePaymentVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "REMOVE PAYMENT";
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="StudentID")
                    RefreshRecentPayments();
                };
        }

        protected override void CreateCommands()
        {
            DeletePaymentCommand = new RelayCommand(async o =>
            {
                if (MessageBoxResult.Yes==MessageBox.Show("Are you sure you would like to delete this payment: \r\nStudent ID:"
                    +selectedPayment.StudentID+"\r\nAmount:"+selectedPayment.AmountPaid,"Warning",MessageBoxButton.YesNo,MessageBoxImage.Warning))
                {
                    bool succ = await DataAccess.RemovePaymentAsync(selectedPayment.FeePaymentID);
                    MessageBox.Show("Succesfully completed operation.");
                    Reset();
                }

            }, o => CanDelete());
        }

        public ObservableCollection<FeePaymentModel> RecentPayments
        {
            get { return recentPayments; }
            set
            {
                if (value != recentPayments)
                {
                    recentPayments = value;
                    NotifyPropertyChanged("RecentPayments");
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

        public FeePaymentModel SelectedPayment
        {
            get { return selectedPayment; }
            set
            {
                if (value != selectedPayment)
                {
                    selectedPayment = value;
                    NotifyPropertyChanged("SelectedPayment");
                }
            }
        }

        private bool CanDelete()
        {
            selectedStudent.CheckErrors();
            
            return !selectedStudent.HasErrors &&
            (selectedPayment != null) && (selectedPayment.FeePaymentID > 0);
        }

        private async void RefreshRecentPayments()
        {
            RecentPayments = await DataAccess.GetRecentPaymentsAsync(selectedStudent);
        }

        public ICommand DeletePaymentCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            selectedStudent.Reset();
        }
    }
}
