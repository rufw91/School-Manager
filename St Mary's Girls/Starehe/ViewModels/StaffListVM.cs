using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Data;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StaffListVM : ViewModelBase
    {
        ObservableCollection<StaffModel> allStaff;
        string searchText;
        CollectionViewSource collViewSource;
        public StaffListVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            StaffDetailsCommand = new RelayCommand(o =>
            {
                if (OpenTaskWindowAction != null)
                    OpenTaskWindowAction.Invoke(new StaffDetailsVM(o as StaffModel));
            }, o => (o is StaffModel));
            RefreshCommand = new RelayCommand(o => { Reset(); }, o => true);
        }

        protected async override void InitVars()
        {
            Title = "STAFF LIST";
            collViewSource = new CollectionViewSource();
            SearchText = "";
            allStaff = await DataAccess.GetAllStaffAsync();
            CollViewSource.Source = allStaff;

        }
        public async override void Reset()
        {
            allStaff = await DataAccess.GetAllStaffAsync();
            CollViewSource.Source = allStaff;
            searchText = "";
        }
        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public CollectionViewSource CollViewSource
        {
            get
            {
                return collViewSource;
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    searchText = value;
                    RenewFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
                else
                {
                    if (searchText != null)
                        RemoveFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
            }
        }

        private void RenewFilter()
        {
            RemoveFilter();
            CollViewSource.Filter += new FilterEventHandler(Filter);
        }
        private void RemoveFilter()
        {
            CollViewSource.Filter -= new FilterEventHandler(Filter);
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = false;
            var src = e.Item as StaffModel;
            if (src == null)
            {
                e.Accepted = false;
            }
            else if (DataAccess.SearchAllStaffProperties(src, SearchText))
                e.Accepted = true;
            else e.Accepted = false;
        }

        public Action<ViewModelBase> OpenTaskWindowAction
        { get; internal set; }

        public ICommand StaffDetailsCommand
        {
            get;
            private set;
        }

    }
}
