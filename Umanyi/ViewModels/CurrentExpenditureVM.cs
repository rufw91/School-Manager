using Helper;
using Helper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class CurrentExpenditureVM:ViewModelBase
    {
        private BudgetModel currentBudget;
        private CollectionViewSource entries;
        public CurrentExpenditureVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "CURRENT EXPENDITURE";
            entries = new CollectionViewSource();
            CurrentBudget = await DataAccess.GetCurrentBudgetAsync();            
            entries.Source = currentBudget.Entries;
            entries.GroupDescriptions.Add(new PropertyGroupDescription("AccountID"));
            NotifyPropertyChanged("Entries");
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                CurrentBudget = await DataAccess.GetCurrentBudgetAsync();
                entries = new CollectionViewSource();
                entries.Source = currentBudget.Entries;
                entries.GroupDescriptions.Add(new PropertyGroupDescription("AccountID"));
                NotifyPropertyChanged("Entries");
                IsBusy = false;
            }, o => CanRefresh());
        }

        private bool CanRefresh()
        {
            return !IsBusy && true;
        }

        public BudgetModel CurrentBudget
        {
            get { return currentBudget; }
            private set
            {
                if (value != this.currentBudget)
                {
                    this.currentBudget = value;
                    NotifyPropertyChanged("CurrentBudget");
                }
            }
        }

        public CollectionViewSource Entries
        {
            get { return this.entries; }
        }


        public ICommand PrintCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }
    }
}
