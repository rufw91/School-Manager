using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class IssueItemsVM: ViewModelBase
    {
        IssueModel newIssue;
        public IssueItemsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "Receive Items";
            IsBusy = true;
            newIssue = new IssueModel();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewItemIssueAsync(newIssue);
                 MessageBox.Show(succ ? "Successfully saved details." : "Could not save details please ensure you have filled all entries correctly.",
                        succ ? "Success" : "Error", MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
               
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
            bool succ = true;
            foreach (var i in newIssue.Items)
                if (i.Quantity==0)
                { succ = false; break; }
            return !string.IsNullOrWhiteSpace(newIssue.Description) && succ &&
                    newIssue.Items.Count > 0;
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

        public IssueModel NewIssue
        {
            get { return this.newIssue; }

            set
            {
                if (value != this.newIssue)
                {
                    this.newIssue = value;
                    NotifyPropertyChanged("NewIssue");
                }
            }
        }

        public override void Reset()
        {
            newIssue.Reset();
        }
    }
}
