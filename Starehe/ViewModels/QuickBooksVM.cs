using Helper;
using Helper.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class QuickBooksSyncVM:ViewModelBase
    {
        public QuickBooksSyncVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "QUICKBOOKS SYNC";
        }

        protected override void CreateCommands()
        {
            SyncCommand = new RelayCommand(async o =>
             {
                 IsBusy = true;
                 bool succ = await QBSyncHelper.SyncInvoice(new SaleModel());
                 IsBusy = false;
                 MessageBox.Show(succ ? "Successfully completed operation." : "An error occured. The operation could not be completed.",
                     succ ? "Success" : "Error", MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                 if (succ)
                     Reset();
             }, o => CanSync());
        }

        private bool CanSync()
        {
            return !IsBusy;
        }

        public ICommand SyncCommand
        { get; private set; }

        public override void Reset()
        {
            IsBusy = false;
        }
    }
}
