using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role="Accounts")]
    public class ReceiveItemsVM: ViewModelBase
    {
        PurchaseModel newReceipt;
        ObservableCollection<SupplierBaseModel> allSuppliers;
        public ReceiveItemsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "NEW PURCHASE";
            IsBusy = true;
            NewReceipt = new PurchaseModel();
            AllSuppliers = await DataAccess.GetAllSuppliersAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewPurchaseAsync(newReceipt);
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

            FindItemsCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                if (FindItemsAction != null)
                {
                    FindItemsAction.Invoke();
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


        public Action FindItemsAction
        { get; internal set; }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand FindItemsCommand
        {
            get;
            private set;
        }

        public PurchaseModel NewReceipt
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
