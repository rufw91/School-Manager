using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
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
            NewStockTaking = new StockTakingModel();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewStockTakingAsync(newStockTaking);
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
            return newStockTaking.DateTaken != null && newStockTaking.DateTaken <= DateTime.Now && 
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
