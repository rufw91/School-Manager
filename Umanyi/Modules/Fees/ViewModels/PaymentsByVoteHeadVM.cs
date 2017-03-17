using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentsByVoteHeadVM:ViewModelBase
    {
        int selectedClassID;
        ObservableCollection<VoteHeadModel> allVoteHeads;
        private ObservableCollection<ClassModel> allClasses;
        private TermModel selectedTerm;
private  ObservableCollection<TermModel> allTerms;
        public PaymentsByVoteHeadVM()
        {
            InitVars();
            CreateCommands();
            PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "SelectedClassID"||e.PropertyName=="SelectedTerm")
                {
                    allVoteHeads.Clear();
                    if (selectedClassID != 0&&selectedTerm!=null)
                    {
                        AllVoteHeads = await DataController.GetVoteHeadsSummaryByClass(selectedClassID,selectedTerm);
                    }
                    return;
                }
                };
        }
        protected async override void InitVars()
        {
            Title = "PAYMENTS BY VOTE HEAD";
            allVoteHeads = new ObservableCollection<VoteHeadModel>();
            AllClasses = await DataController.GetAllClassesAsync();
            AllTerms = await DataController.GetAllTermsAsync();
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                AllVoteHeads = await DataController.GetVoteHeadsSummaryByClass(selectedClassID,selectedTerm);
            }, o => Canrefresh());
        }

        private bool Canrefresh()
        {
            return selectedClassID != 0&&selectedTerm!=null;
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

        public TermModel SelectedTerm
        {
            get { return selectedTerm; }
            set
            {
                if (value != selectedTerm)
                {
                    selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
        }

        public ObservableCollection<TermModel> AllTerms
        {
            get { return allTerms; }
            private set
            {
                if (value != allTerms)
                {
                    allTerms = value;
                    NotifyPropertyChanged("AllTerms");
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
