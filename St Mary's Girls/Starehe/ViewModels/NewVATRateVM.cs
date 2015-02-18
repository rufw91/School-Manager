using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewVATRateVM : ViewModelBase
    {
        VATRateModel newRate;
        public NewVATRateVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "New VAT Rate";
            IsBusy = true;
            NewRate = new VATRateModel();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (!newRate.HasErrors)
                {
                    bool res = await DataAccess.SaveNewVATRateAsync(newRate);
                    if (res)
                    {
                        MessageBox.Show("Successfully Completed Operation.");
                        Reset();
                    }
                    else
                        MessageBox.Show("Could not save new item.");
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(newRate.Description)&&newRate.Rate>=0;
        }

        public VATRateModel NewRate
        {
            get { return newRate; }
            set
            {
                if (newRate != value)
                {
                    newRate = value;
                    NotifyPropertyChanged("NewRate");
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
            NewRate.Reset();
        }
    }
}
