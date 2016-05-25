using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class AccountsVM : ViewModelBase
    {
        public AccountsVM()
        {
            InitVars();
            CreateCommands(); 
        }
        public ICommand StartCommand
        { get; private set; }

        public ICommand OpenTaskWindowCommand
        { get; private set; }
                
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
            OpenTaskWindowCommand = new RelayCommand(o =>
            {
                if (OpenTaskWindowAction != null)
                {
                    OpenTaskWindowAction.Invoke(o as ViewModelBase);
                }
            }, o => CanStart());
            StartCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                OpenTaskWindowCommand.Execute(new AccountsMainVM());
                IsBusy = false;
            }, o => CanStart());

        }

        private bool CanStart()
        {
            return !IsBusy&&OpenTaskWindowAction != null;
        }

        protected override void InitVars()
        {
            Title = "ACCOUNTS";
        }
        
        public Action<ViewModelBase> OpenTaskWindowAction
        { get; internal set; }

    }
}
