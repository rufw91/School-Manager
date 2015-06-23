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
    public class ReceiveBooksVM: ViewModelBase
    {
        BooksPurchaseModel newReceipt;
        ObservableCollection<SupplierBaseModel> allSuppliers;
        public ReceiveBooksVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "Receive Books";
            IsBusy = true;
            NewReceipt = new BooksPurchaseModel();
            AllSuppliers = await DataAccess.GetAllSuppliersAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewBooksPurchaseAsync(newReceipt);
                if (succ)
                {
                    MessageBox.Show("Successfully saved purchase information.");
                    Reset();
                }
                else
                {
                    MessageBox.Show("Could not save details please ensure you have filled all entries correctly.");                    
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());

            FindBooksCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                if (FindBooksAction != null)
                {
                    FindBooksAction.Invoke();
                }
                IsBusy = false;
            }, o => !IsBusy);
        }

        private bool CanSave()
        {
            bool succ = true;
            foreach (var i in newReceipt.Items)
                if (i.TotalAmt==0)
                { succ = false; break; }
            return newReceipt.SupplierID > 0 && succ&&!string.IsNullOrWhiteSpace(newReceipt.RefNo)&&
                    newReceipt.Items.Count > 0 && newReceipt.OrderTotal > 0;
        }

        public ObservableCollection<SupplierBaseModel> AllSuppliers
        {
            get { return allSuppliers; }

            private set
            {
                if (value != this.allSuppliers)
                {
                    this.allSuppliers = value;
                    NotifyPropertyChanged("AllSuppliers");
                }
            }
        }


        public Action FindBooksAction
        { get; internal set; }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand FindBooksCommand
        {
            get;
            private set;
        }

        public BooksPurchaseModel NewReceipt
        {
            get { return this.newReceipt; }

            set
            {
                if (value != this.newReceipt)
                {
                    this.newReceipt = value;
                    NotifyPropertyChanged("NewReceipt");
                }
            }
        }

        public override void Reset()
        {
            NewReceipt.Reset();
        }
    }
}
