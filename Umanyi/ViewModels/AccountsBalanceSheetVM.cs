using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AccountsBalanceSheetVM : ViewModelBase
    {
        private FixedDocument fd;
        public AccountsBalanceSheetVM()
        {
            InitVars();
            CreateCommands();            
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                BalanceSheetModel im = new BalanceSheetModel();
                Document = DocumentHelper.GenerateDocument(im);
                IsBusy = false;
            }, o => true);
        }

        protected override void InitVars()
        {
            Title = "BALANCE SHEET";
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }
    }
}
