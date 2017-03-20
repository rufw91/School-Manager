using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StockTakingVM : ViewModelBase
    {
        StockTakingModel newStockTaking;
        public StockTakingVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "Stock Taking";
            IsBusy = false;
            NewStockTaking = new StockTakingModel();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataController.SaveNewStockTakingAsync(newStockTaking);
                if (succ)
                {
                    MessageBox.Show("Successfully saved information.");
                    Reset();
                }
                else
                {
                    MessageBox.Show("Could not save details please ensure you have filled all entries correctly.");
                    return;
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
            Debug.WriteLine("" + newStockTaking.Items.Count);
            return newStockTaking.DateTaken != null && newStockTaking.DateTaken.Value <= DateTime.Now && 
                    newStockTaking.Items.Count > 0;
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

        public StockTakingModel NewStockTaking
        {
            get { return this.newStockTaking; }

            set
            {
                if (value != this.newStockTaking)
                {
                    this.newStockTaking = value;
                    NotifyPropertyChanged("NewStockTaking");
                }
            }
        }

        public override void Reset()
        {
            NewStockTaking.Reset();
        }
    }
}
