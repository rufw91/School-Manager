using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AccountsGeneralLedgerVM : ViewModelBase
    {
        private FixedDocument fd;
        private DateTime from;
        private DateTime to;

        public AccountsGeneralLedgerVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(async o =>
            {
                var temp = await DataAccess.GetGeneralLedgerAsync(SelectedAccount, from, to);
                var doc = DocumentHelper.GenerateDocument(temp);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGenerate() && Document != null);
            GenerateCommand = new RelayCommand(async o =>
            {
            var temp = await DataAccess.GetGeneralLedgerAsync(SelectedAccount, from, to);
                Document = DocumentHelper.GenerateDocument(temp);
            },
               o => CanGenerate());
        }

        private bool CanGenerate()
        {
            return true;
        }

        protected override void InitVars()
        {
            Title = "GENERAL LEDGER";
            AllAccounts = Enum.GetValues(typeof(GeneralLedgerAccounts));
            From = DateTime.Now.AddDays(-30);
            To = DateTime.Now;
        }


        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public Array AllAccounts
        {
            get;
            private set;
        }

        public GeneralLedgerAccounts SelectedAccount
        {
            get;
            private set;
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
        public DateTime From
        {
            get { return this.from; }

            set
            {
                if (value != this.from)
                {
                    this.from = value;
                    NotifyPropertyChanged("From");
                }
            }
        }

        public DateTime To
        {
            get { return this.to; }

            set
            {
                if (value != this.to)
                {
                    this.to = value;
                    NotifyPropertyChanged("To");
                }
            }
        }
    }
}
