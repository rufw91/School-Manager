using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentsByVoteHeadVM:ViewModelBase
    {
        int selectedClassID;
        ObservableCollection<VoteHeadModel> allVoteHeads;
        private ObservableCollection<ClassModel> allClasses;
        public PaymentsByVoteHeadVM()
        {
            InitVars();
            CreateCommands();
            PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "SelectedClassID")
                {
                    allVoteHeads.Clear();
                    if (SelectedClassID != 0)
                    {
                        AllVoteHeads = await DataAccess.GetVoteHeadsSummaryByClass(selectedClassID);
                    }
                    return;
                }
                };
        }
        protected async override void InitVars()
        {
            Title = "PAYMENTS BY VOTE HEAD";
            allVoteHeads = new ObservableCollection<VoteHeadModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();            
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                AllVoteHeads = await DataAccess.GetVoteHeadsSummaryByClass(selectedClassID);
            }, o => Canrefresh());
        }

        private bool Canrefresh()
        {
            return SelectedClassID != 0;
        }
        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }
        public ObservableCollection<VoteHeadModel> AllVoteHeads
        {
            get { return allVoteHeads; }
            private set
            {
                if (allVoteHeads != value)
                {
                    allVoteHeads = value;
                    NotifyPropertyChanged("AllVoteHeads");
                }
            }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            private set
            {
                if (allClasses != value)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        public ICommand RefreshCommand
        {
            private get;
            set;
        }
        public override void Reset()
        {
            allVoteHeads.Clear();
            selectedClassID = 0;
        }
    }
}
