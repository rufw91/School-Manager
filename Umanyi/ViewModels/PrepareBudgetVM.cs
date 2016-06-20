using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class PrepareBudgetVM:ViewModelBase
    {
        private BudgetModel newBudget;
        private CollectionViewSource entries;
        public PrepareBudgetVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "PREPARE BUDGET";
            entries = new CollectionViewSource();
            NewBudget = await DataAccess.GetCurrentBudgetAsync();
            
            entries.Source = newBudget.Entries;
            entries.GroupDescriptions.Add(new PropertyGroupDescription("AccountID"));
            NotifyPropertyChanged("Entries");
        }

        protected override void CreateCommands()
        {
            AddItemsCommand = new RelayCommand(o => 
            {
                IsBusy = true;
                if (FindItemsAction != null)
                {
                    FindItemsAction.Invoke();
                }
                IsBusy = false;
            },o=> CanAdd());

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewBudgetAsync(newBudget);
                if (succ)
                    Reset();
                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details.", succ ? "Success" : "Error", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                IsBusy = true;
            }, o =>CanSave());
        }

        private bool CanSave()
        {
            return !IsBusy;
        }

        private bool CanAdd()
        {
            if (newBudget!=null)
            newBudget.RefreshValues();
            return true;
        }

        public CollectionViewSource Entries
        {
            get { return this.entries; }
        }

        public Action FindItemsAction
        { get; internal set; }

        public BudgetModel NewBudget
        {
            get { return this.newBudget; }

            set
            {
                if (value != this.newBudget)
                {
                    this.newBudget = value;
                    NotifyPropertyChanged("NewBudget");
                }
            }
        }

        public ICommand AddItemsCommand
        { get; private set; }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
        }
    }
}
