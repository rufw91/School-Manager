using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class VATAnalysisVM : ViewModelBase
    {
        DateTime from;
        DateTime to;
        bool _canExport;
        ObservableCollection<VATAnalysisModel> items;
        public VATAnalysisVM()
        {
            CanExport = false;
            InitVars();
            CreateCommands();
        }

        public VATAnalysisVM(bool canExport)
        {
            CanExport = canExport;
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            IsBusy = true;
            Title = "VAT Analysis";
            items = new ObservableCollection<VATAnalysisModel>();
            From = DateTime.Now.Date.AddDays(-1);
            To = DateTime.Now;
            IsBusy = false;
        }
        
        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => { IsBusy = true; Items = await DataAccess.GetVatAnalysisAsync(from, to); IsBusy = false; }, o => !IsBusy && from < to);
            ExportCommand = new RelayCommand(o => { }, o => !IsBusy&&iCanExport());
        }

        private DataTable GetTable(ObservableCollection<VATAnalysisModel> Items)
        {
            DataTable dt = new DataTable();
            DataColumn c1 = new DataColumn("VAT Code",typeof(int));
            DataColumn c2 = new DataColumn("Description",typeof(string));
            DataColumn c3 = new DataColumn("Rate",typeof(decimal));
            DataColumn c4 = new DataColumn("Total VAT Collected",typeof(decimal));

            dt.Columns.AddRange(new DataColumn[] { c1, c2, c3, c4 });
            DataRow dtr;
            foreach(VATAnalysisModel v in items)
            {
                dtr = dt.NewRow();
                dtr[0] = v.VatID;
                dtr[1] = v.Description;
                dtr[2] = v.Rate;
                dtr[3] = v.TotalVATCollected;

                dt.Rows.Add(dtr);
            }
            return dt;
        }

        private bool iCanExport()
        {
            return CanExport && Items.Count > 0;
        }

        public override void Reset()
        {
        }

        public ICommand ExportCommand
        { get; private set; }

        public ICommand RefreshCommand
        { get; private set; }

        public bool CanExport
        {
            get { return this._canExport; }

            private set
            {
                if (value != this._canExport)
                {
                    this._canExport = value;
                    NotifyPropertyChanged("CanExport");
                }
            }
        }

        public DateTime From
        {
            get { return this.from; }

            private set
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

            private set
            {
                if (value != this.to)
                {
                    this.to = value;
                    NotifyPropertyChanged("To");
                }
            }
        }

        public ObservableCollection<VATAnalysisModel> Items
        {
            get { return this.items; }

            private set
            {
                if (value != this.items)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }
    }
}
