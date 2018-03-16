using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ItemListVM: ViewModelBase
    {
        ObservableCollection<ItemModel> allItems;
        public ItemListVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "ITEMS LIST";
            allItems = await DataController.GetAllItemsAsync();
            NotifyPropertyChanged("AllItems");
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(o => { IsBusy = true; Reset(); IsBusy = false; }, o => !IsBusy);
            
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }
        
        public ObservableCollection<ItemModel> AllItems
        {
            get { return this.allItems; }
        }

        public async override void Reset()
        {
            IsBusy = true;
            allItems= await DataController.GetAllItemsAsync();
            NotifyPropertyChanged("AllItems");
            IsBusy = false;
        }
        
    }

}
